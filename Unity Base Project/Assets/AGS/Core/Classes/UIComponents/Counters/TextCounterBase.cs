using System.Globalization;
using UnityEngine.UI;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This abstract UI component handles integer and displays it using UI.Text.
    /// Override SetupCounter to set specific int counter. Call UpdateGUIText() from implementation to update UI.Text
    /// </summary>
    public abstract class TextCounterBase : UIScriptBase
    {
        public Text Text;
        protected int Counter;


        public override void Awake()
        {
            base.Awake();
            Text = GetComponent<Text>();
        }

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();
            SetupCounter();            
        }

        /// <summary>
        /// Setups the counter.
        /// </summary>
        protected abstract void SetupCounter();

        /// <summary>
        /// Updates the GUI text.
        /// </summary>
        protected void UpdateGUIText()
        {
            string counterText;
            if (Counter >= 10)
            {
                counterText = Counter.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                counterText = string.Format("0{0}", Counter.ToString(CultureInfo.InvariantCulture));
            }
            Text.text = counterText;
        }
    }
}
