using UnityEngine;
using UnityEngine.UI;
using System;
using Battlehub.UIControls.Dialogs;
using System.Linq;
using thelab.mvc;
using Battlehub.UIControls.DockPanels;

namespace FootTactic
{
    public class FTDockPanelController : Controller<FTApplication>
    {
        [SerializeField]
        private DialogManager m_dialog = null;

        [SerializeField]
        private DockPanel m_dockPanels = null;

        [SerializeField]
        private Sprite m_spriteTeam = null;

        [SerializeField]
        private Sprite m_spritePlayer = null;

        [SerializeField]
        private string m_headerText = null;

        [SerializeField]
        private Transform m_contentPrefabPlayer = null;

        [SerializeField]
        private Transform m_contentPrefabTeam = null;


        [SerializeField]
        private RegionSplitType m_splitType = RegionSplitType.None;

        private int m_counter;

        
        private void OnShowDialogPlayer()
        {
            m_counter++;

            Transform content = Instantiate(m_contentPrefabPlayer);
            var editPlayer = content.GetComponent<EditPlayerView>();
            int playerTeamIndex = FTUtil.GetPlayerTeamIndex(app.controller.selectedPlayer.PlayerIndex);
            editPlayer.InitializedFields( app.model.Tactic.Teams[getTeamIndex()].Players[playerTeamIndex]);
            Dialog dlg = m_dialog.ShowDialog(m_spritePlayer, "Player Editor " + m_counter, content, (sender, okArgs) =>
            {
                Debug.Log("Saving edit Player");
                Debug.Log(editPlayer.HairColor.value+" <-- hair color");
                app.controller.selectedPlayer.PlayerStyle.ApplyStyle(editPlayer);
                int playerTeam = FTUtil.GetTeamIndex(app.controller.selectedPlayer.PlayerIndex);
                app.model.Tactic.Teams[playerTeam].Players[FTUtil.GetPlayerTeamIndex(app.controller.selectedPlayer.PlayerIndex)].UpdateStyleAttributes(editPlayer.Hair.value,editPlayer.Skin.value, editPlayer.HairColor.value, editPlayer.Face.value, editPlayer.Tatoo.value, editPlayer.Beard.value);

            }, "Save", (sender, cancelArgs) =>
            {                
                Debug.Log("Cancelled Edit Player");
            }, "Cancel");

            dlg.IsOkVisible = true;
            dlg.IsCancelVisible = true;
        }

        private void OnShowDialogTeam()
        {
            m_counter++;

            Transform content = Instantiate(m_contentPrefabTeam);
            var editTeam = content.GetComponent<EditTeamView>();
            editTeam.UpdateColorsToCurrentTeam(app.model.Tactic.GetTeamColors(getTeamIndex()));
            editTeam.TeamName.text = app.model.Tactic.GetTeamName(getTeamIndex());
            Dialog dlg = m_dialog.ShowDialog(m_spriteTeam, "Team Editor " + m_counter, content, (sender, okArgs) =>
            {
                app.controller.selectedPlayer.PlayerStyle.SetTeamStyle(editTeam);               

            }, "Save", (sender, cancelArgs) =>
            {
                Debug.Log("NO");
            }, "Cancel");

            dlg.IsOkVisible = true;
            dlg.IsCancelVisible = true;
        }

        private void OnShowMsgBox()
        {
            m_dialog.ShowDialog(m_spriteTeam, "Msg Test", "Your message", (sender, okArgs) =>
            {
                Debug.Log("YES");
                //OnShowMsgBox();
                //okArgs.Cancel = true;

            }, "Yes", (sender, cancelArgs) =>
            {
                Debug.Log("NO");
            }, "No");
        }


        private void OnDefaultLayout()
        {
            Region rootRegion = m_dockPanels.RootRegion;
            rootRegion.Clear();
            foreach (Transform child in m_dockPanels.Free)
            {
                Region region = child.GetComponent<Region>();
                region.Clear();
            }

            LayoutInfo layout = new LayoutInfo(false,
                new LayoutInfo(Instantiate(m_contentPrefabTeam).transform, m_headerText + " " + m_counter++, m_spriteTeam),
                new LayoutInfo(true,
                    new LayoutInfo(Instantiate(m_contentPrefabTeam).transform, m_headerText + " " + m_counter++, m_spriteTeam),
                    new LayoutInfo(Instantiate(m_contentPrefabTeam).transform, m_headerText + " " + m_counter++, m_spriteTeam),
                    0.5f),
                0.75f);

            m_dockPanels.RootRegion.Build(layout);
        }

        public override void OnNotification(string p_event, UnityEngine.Object p_target, params object[] p_data)
        {
            switch (p_event)
            {
                case "EditPlayer":
                    OnShowDialogPlayer();
                    break;

                case "EditTeam":
                    OnShowDialogTeam();
                    break;

            }
        }

        int getTeamIndex()
        {
            return app.controller.selectedPlayer.PlayerIndex >= FTConstants.TEAM2_START_INDEX ? 1 : 0;
        }
    }
}
