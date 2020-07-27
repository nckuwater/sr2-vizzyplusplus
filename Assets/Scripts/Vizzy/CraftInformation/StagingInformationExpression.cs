using System;
using System.Collections.Generic;
using ModApi.Craft.Program;
using ModApi.Craft.Parts;
using ModApi.Flight;
using Assets.Scripts.Vizzy;
using UnityEngine;

namespace Assets.Scripts.Vizzy.CraftInformation{
    [Serializable]
    public class StagingInformationExpression : ProgramExpression{
        public override bool IsBoolean => false;
        public const String XmlName = "StagingInformation";
        [ProgramNodeProperty]
        private string _type;
        public static void Initialize(){

        }
        public override ExpressionResult Evaluate(IThreadContext context){
            switch(_op){
                case(StagingInformationType.Current):
                    return new ExpressionResult {
                        NumberValue = context.Craft.CraftScript.ActiveCommandPod.currentStage /// from 0
                    };
                case(StagingInformationType.Remain):
                    return new ExpressionResult {
                        context.Craft.CraftScript.ActiveCommandPod.NumStages
                    };
                case(StagingInformationType.Parts):
                    /// List<string>
                    List<string> partlist = new List<string>();
                    foreach(PartData partGroup in context.Craft.CraftScript.RootPart.BodyScript.PartGroups){
                        foreach(IPartGroupScript part in partGroup.Data.Parts){
                            if (part.ActivationStage == GetExpression(0).Evaluate(context).NumberValue){
                                partlist.Add(Convert.ToString(part.id));
                            }
                        }
                    }
                    return new ExpressionResult{
                        ListValue = partlist
                    };
            }
        }

    }
    public enum StagingInformationType{
        Current = "current",
        Remain = "remain",
        Parts = "parts"
    }
}