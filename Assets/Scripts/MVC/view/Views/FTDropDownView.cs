using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;

namespace FootTactic
{
    public class FTDropDownView : ButtonView<FTApplication>
    {
        /// <summary>
        /// Reference to the component.
        /// </summary>
        public Dropdown dropdown;

        /// <summary>
        /// CTOR.
        /// </summary>
        protected void Awake()
        {
            dropdown = GetComponent<Dropdown>();
            if (dropdown) dropdown.onValueChanged.AddListener(OnChange);
        }

        /// <summary>
        /// Callback for value change on component.
        /// </summary>
        /// <param name="v"></param>
        virtual protected void OnChange(int v)
        {
            Notify(notification + "@change", this, v);
        }
    }

}

