//-----------------------------------------------------------------------
// <copyright file="WizardManagerBase.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sphere10.Framework;

namespace Sphere10.Framework.Windows.Forms {
    public abstract class WizardManagerBase<T> : Disposable, IWizardManager<T> {
        public event EventHandlerEx Finished;
        private Form _owner;
        private WizardDialog<T> _dialog;
        private WizardScreen<T> _currentVisibleScreen;
        private readonly List<WizardScreen<T>> _screens;
        private int _currentScreenIndex;
        private bool _started;
        private string _nextText;
        private string _finishText;
        private string _title;

        protected WizardManagerBase(string title, T propertyBag, IEnumerable<WizardScreen<T>> screens, string finishText = null) {
            var wizardScreens = screens as IList<WizardScreen<T>> ?? screens.ToList();
            if (!wizardScreens.Any())
                throw new ArgumentOutOfRangeException("screens", "Wizard needs at least 1 screen");
            _screens = wizardScreens.ToList();
            PropertyBag = propertyBag;
            _started = false;
            _nextText = "Next";
            _finishText = finishText ?? "Finish";
            Title = title;
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (_dialog != null) {
                    _dialog.Text = _title;
                }
            }
        }

        public T PropertyBag { get; private set; }

        public bool HasNext {
            get {
                CheckStarted();
                return _currentScreenIndex < _screens.Count - 1;
            }
        }

        public bool HasPrevious {
            get {
                CheckStarted();
                return _currentScreenIndex > 0;
            }
        }

        public bool HideNext {
            get {
                CheckStarted();
                return _dialog._nextButton.Enabled;
            }
            set {
                CheckStarted();
                _dialog._nextButton.Enabled = value;
            }
        }

        public bool HidePrevious {
            get {
                CheckStarted();
                return _dialog._previousButton.Visible;
            }
            set {
                CheckStarted();
                _dialog._previousButton.Visible = !value;
            }
        }

        public string NextText {
            get {
                CheckStarted();
                return _dialog._nextButton.Text;
            }
            set {
                CheckStarted();
                _dialog._nextButton.Text = value;
            }
        }

        protected override void FreeManagedResources() {
            foreach (var screen in _screens) {
                screen.Dispose();
            }
        }

        public async Task Start(Form parent) {
            CheckNotStarted();
            _owner = parent;
            _dialog = new WizardDialog<T>();
            _dialog.Text = _title;
            _dialog.WizardManager = this;
            _dialog.FormClosing += (sender, args) => { };
            _dialog.FormClosed += (sender, args) => { };
            _currentScreenIndex = 0;
            _started = true;
            foreach (var screen in _screens)
                await screen.Initialize();
            await PresentScreen(_screens[_currentScreenIndex]);
            _dialog.Show(_owner);
        }

        public async Task Next() {
            CheckStarted();
            var validation = await _currentVisibleScreen.Validate();
            if (validation.Failure) {
                DialogEx.Show(_owner, SystemIconType.Error, "Error", validation.ErrorMessages.ToParagraphCase(), "OK");
                return;
            }
            await _currentVisibleScreen.OnNext();
            if (HasNext) {
                await PresentScreen(_screens[++_currentScreenIndex]);
            } else {
                var finishResult = await Finish();
                if (finishResult.Failure) {
                    MessageBox.Show(_owner, finishResult.ErrorMessages.ToParagraphCase(true), "Error");
                    return;
                }
                FireFinishedEvent();
                _dialog.CloseDialog();
                Dispose();
            }
        }

        public async Task Previous() {
            CheckStarted();
            if (!HasPrevious)
                return;
            await _currentVisibleScreen.OnPrevious();
            await PresentScreen(_screens[--_currentScreenIndex]);
        }

        public virtual Result CancelRequested() {
            return Result.Default;
        }

        public void RemoveSubsequentScreensOfType(Type type) {
            var formsToRemove = new List<WizardScreen<T>>();
            for (int i = _currentScreenIndex + 1; i < _screens.Count; i++)
                if (_screens[i].GetType() == type)
                    formsToRemove.Add(_screens[i]);
            formsToRemove.ForEach(f => _screens.Remove(f));
        }

        public async Task InjectScreen(WizardScreen<T> screen) {
            CheckStarted();
            await screen.Initialize();
            _screens.Insert(_currentScreenIndex + 1, screen);
            NextText = !HasNext ? _finishText : _nextText;
        }

        public void RemoveScreen(WizardScreen<T> screen) {
            _screens.Remove(screen);
        }

        protected abstract Task<Result> Finish();

        private void FireFinishedEvent() {
            if (Finished != null)
                Finished();
        }

        private async Task PresentScreen(WizardScreen<T> screen) {
            _currentVisibleScreen = screen;
            _currentVisibleScreen.Wizard = this;
          
            HidePrevious = !HasPrevious;
            NextText = !HasNext ? _finishText : _nextText;

            await _dialog.SetContent(screen);
            await screen.OnPresent();            
        }

        private void CheckStarted() {
            if (!_started)
                throw new InvalidOperationException("Wizard has not been started");
        }
        private void CheckNotStarted() {
            if (_started)
                throw new InvalidOperationException("Wizard has already been started");
        }
    }
}
