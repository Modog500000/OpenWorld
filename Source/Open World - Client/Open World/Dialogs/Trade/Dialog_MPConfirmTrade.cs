﻿using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace OpenWorld
{
    public class Dialog_MPConfirmTrade : Window
    {
        public override Vector2 InitialSize => new Vector2(350f, 150f);

        private string windowTitle = "Warning!";
        private string windowDescription = "Are you sure you want to trade these items?";

        private float buttonX = 137f;
        private float buttonY = 38f;

        public Dialog_MPConfirmTrade()
        {
            soundAppear = SoundDefOf.CommsWindow_Open;
            soundClose = SoundDefOf.CommsWindow_Close;
            absorbInputAroundWindow = true;
            closeOnAccept = false;
            closeOnCancel = true;
        }

        public override void DoWindowContents(Rect rect)
        {
            float centeredX = rect.width / 2;

            float windowDescriptionDif = Text.CalcSize(windowDescription).y + StandardMargin;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(centeredX - Text.CalcSize(windowTitle).x / 2, rect.y, Text.CalcSize(windowTitle).x, Text.CalcSize(windowTitle).y), windowTitle);

            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(centeredX - Text.CalcSize(windowDescription).x / 2, windowDescriptionDif, Text.CalcSize(windowDescription).x, Text.CalcSize(windowDescription).y), windowDescription);

            if (Widgets.ButtonText(new Rect(new Vector2(rect.x, rect.yMax - buttonY), new Vector2(buttonX, buttonY)), "OK"))
            {
                if (string.IsNullOrWhiteSpace(Main._ParametersCache.wantedSilver) || int.Parse(Main._ParametersCache.wantedSilver) == 0) return;

                if (!Networking.isConnectedToServer)
                {
                    Find.WindowStack.Add(new OW_ErrorDialog("Connection timeout, please check and try again"));
                    return;
                }

                MPCaravan.TakeItemsFromCaravan(1);

                TradeHandler.SendTradesToSettlement();

                Close();
            }

            if (Widgets.ButtonText(new Rect(new Vector2(rect.xMax - buttonX, rect.yMax - buttonY), new Vector2(buttonX, buttonY)), "Cancel"))
            {
                Close();
            }
        }
    }
}
