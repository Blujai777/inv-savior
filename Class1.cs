using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using MoreSlugcats;
using RWCustom;
using UnityEngine;

namespace Inv.Savior
{
    [BepInPlugin("blujai.invior", "inv savior", "0.1.0")]
    public class Class1 : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "Blujai.invior"; 
        public const string PLUGIN_NAME = "inv savior"; 
        public const string PLUGIN_VERSION = "0.1.0"; 

        public void OnEnable()
        {

            On.SLOracleBehavior.InitCutsceneObjects += SLOracleBehavior_InitCutsceneObjects;
            On.SLOracleBehaviorHasMark.MoonConversation.AddEvents += SLOracleBehaviorHasMark_MoonConversation_AddEvents;

        }

        private void SLOracleBehaviorHasMark_MoonConversation_AddEvents(On.SLOracleBehaviorHasMark.MoonConversation.orig_AddEvents orig, SLOracleBehaviorHasMark.MoonConversation self)
        {
            Custom.Log(new string[]
            {
                self.id.ToString(),
                self.State.neuronsLeft.ToString()
             });
            if (self.id == Conversation.ID.MoonFirstPostMarkConversation)
            {
                switch (Mathf.Clamp(self.State.neuronsLeft, 0, 5))
                {
                    case 0:
                        break;
                    case 1:
                        
                        return;
                    case 2:
                        
                        return;
                    case 3:
                         return;
                    case 4:
                         return;
                    case 5:
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 5));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("What The Fuck"), 10));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Like actually..."), 5));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("you can do so many things with your life but you choose this"), 10));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("you broke the timeline, hunter is supposed to revive me, not you..."), 10));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("i'm not mad..."), 10));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("just disappointed"), 10));
                        self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("why are you still here?"), 10));
                        return;
                    default:
                        return;
                }
            }
            orig(self);
        }

        private void SLOracleBehavior_InitCutsceneObjects(On.SLOracleBehavior.orig_InitCutsceneObjects orig, SLOracleBehavior self)
        {
            orig(self);

            FieldInfo initWakeUpProcedureField = typeof(SLOracleBehavior).GetField("initWakeUpProcedure", BindingFlags.NonPublic | BindingFlags.Instance);
            bool initWakeUpProcedure = (bool)initWakeUpProcedureField.GetValue(self);

            FieldInfo initRivuletEndingField = typeof(SLOracleBehavior).GetField("initRivuletEnding", BindingFlags.NonPublic | BindingFlags.Instance);
            bool initRivuletEnding = (bool)initRivuletEndingField.GetValue(self);

            if (!initRivuletEnding && ModManager.MSC && self.player != null && self.oracle.room.game.IsStorySession
                && self.oracle.room.game.GetStorySession.saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Rivulet && self.oracle.room.game.IsMoonActive())
                
            {
                if (ModManager.Expedition && self.oracle.room.game.rainWorld.ExpeditionMode)
                {
                    return;
                }

                self.rivEnding = new SLOracleRivuletEnding(self.oracle);
                self.oracle.room.AddObject(self.rivEnding);

                
                initRivuletEndingField.SetValue(self, true);
            }
            if (!initWakeUpProcedure && ModManager.MSC && self.player != null && self.oracle.room.game.IsStorySession
                && self.oracle.room.game.GetStorySession.saveStateNumber == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel && self.State.neuronsLeft < 1)
            {
                self.oracle.room.AddObject(new SLOracleWakeUpProcedure(self.oracle));
                initWakeUpProcedureField.SetValue(self, true);
            }
           
                
           
        }
    }
}
