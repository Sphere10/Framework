//-----------------------------------------------------------------------
// <copyright file="TestBlock.cs" company="Sphere 10 Software">
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
using System.Text;
using Sphere10.Framework.Windows.Forms;

namespace Sphere10.FrameworkTester
{
    public class TestBlock : ApplicationBlock
    {
        public TestBlock()
            : base(
                "Block 1",
                null,
                null,
                null,
                new Menu[] {
	                new Menu(
		                "Tests",
		                null,
		                new ScreenMenuItem[] {
			                new ScreenMenuItem("Test Launcher",typeof(TestLauncherScreen),null),
		                }
	                ),
					new Menu(
                        "Menu 1",
                        null,
                        new ScreenMenuItem[] {
                            new ScreenMenuItem("Option 1",typeof(ScreenA),null),
                            new ScreenMenuItem("Option 2",typeof(ScreenB),null),
                            new ScreenMenuItem("Option 3",typeof(ScreenC),null),
                            new ScreenMenuItem("Option 4",typeof(ScreenA),null)
                        }
                    ),

                   new Menu(
                        "Menu 2",
                        null,
                        new ScreenMenuItem[] {
                            new ScreenMenuItem("Option 1",typeof(ScreenA),null),
                            new ScreenMenuItem("Option 2",typeof(ScreenA),null),
                        }
                    )
                }
             ) 
        {
        }

    }



    public class TestBlock2 : ApplicationBlock
    {
        public TestBlock2()
            : base(
                "Block 2",
                null,
                null,
                null,
                new Menu[] {
                    new Menu(
                        "Menu 1",
                        null,
                        new ScreenMenuItem[] {
                            new ScreenMenuItem("Opt 1",typeof(ScreenA),null),
                            new ScreenMenuItem("Opt 2",typeof(ScreenA),null),
                        }
                    ),

                   new Menu(
                        "Menu 2",
                        null,
                        new ScreenMenuItem[] {
                            new ScreenMenuItem("Opt 1",typeof(ScreenA),null),
                            new ScreenMenuItem("Opt 2",typeof(ScreenA),null),
                            new ScreenMenuItem("Opt 3",typeof(ScreenA),null),
                            new ScreenMenuItem("Opt 4",typeof(ScreenA),null),
                        }
                    )
                }
             ) {
        }

    }
}

