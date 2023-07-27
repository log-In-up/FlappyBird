using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    public sealed class SettingsWindow : ScreenObserver
    {
        public override UIScreen Screen => UIScreen.Settings;
    }
}
