using UnityEngine;
using UnityEngine.EventSystems;
using Battlehub.UIControls.MenuControl;
using thelab.mvc;

namespace FootTactic
{
    public class FTContextMenuTrigger : Controller<FTApplication>
    {
        [SerializeField]
        private Menu m_menu = null;
        [SerializeField]
        private Canvas canvas = null;
        [SerializeField]
        private RectTransform rectTransform = null;

        private void OpenMenu()
        {
            //Canvas canvas = GetComponentInParent<Canvas>();
            Vector3 position;
            Vector2 pos = Input.mousePosition;

            if(canvas.renderMode != RenderMode.ScreenSpaceOverlay && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos, canvas.worldCamera))
            {
                return;
            }
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, pos, canvas.worldCamera, out position))
            {
                m_menu.transform.position = position;
                m_menu.Open();
            }
            
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {

                case "OpenMenu":
                    OpenMenu();
                    break;

            }
        }

        private void Start()
        {
            m_menu.Closed += M_menu_Closed;
        }

        private void M_menu_Closed(object sender, System.EventArgs e)
        {
            app.controller.Notify("ClosedMenu");
        }
    }

}
