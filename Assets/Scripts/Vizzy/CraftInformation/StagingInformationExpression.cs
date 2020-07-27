using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
        private StagingInformationType _infoType;
        public static void Initialize(){

        }
        public override ExpressionResult Evaluate(IThreadContext context){
            //Debug.Log($"cr= {context.Craft.CraftScript.ActiveCommandPod.CurrentStage}");
            switch(_infoType){
                case(StagingInformationType.Current):
                    return new ExpressionResult {
                        NumberValue = context.Craft.CraftScript.ActiveCommandPod.CurrentStage /// from 0
                    };
                case(StagingInformationType.Remain):
                    return new ExpressionResult {
                        NumberValue = context.Craft.CraftScript.ActiveCommandPod.NumStages
                    };
                case(StagingInformationType.Parts):
                    /// List<string>
                    List<string> partlist = new List<string>();
                    /// real stage number = (total - index) due to stage of ActivecommandPod will be 1.
                    //int selectedStage = context.Craft.CraftScript.ActiveCommandPod.NumStages - (int)GetExpression(0).Evaluate(context).NumberValue;
                    int selectedStage = (int)GetExpression(0).Evaluate(context).NumberValue;
                    //Debug.Log($"selected stage= {selectedStage}");
                    //foreach(IPartGroupScript partGroup in context.Craft.CraftScript.RootPart.BodyScript.PartGroups){
                    string partStr;
                    foreach(PartData part in context.Craft.CraftScript.Data.Assembly.Parts){
                        Debug.Log(part.ActivationStage);
                        if (part.ActivationStage == selectedStage){
                            partStr = (Convert.ToString(part.Id)).Trim();
                            Debug.Log($"match {partStr}");
                            partlist.Add(partStr);
                        }
                    }

                    //}
                    return new ExpressionResult(partlist);
                default:
                    return new ExpressionResult{
                        NumberValue = -1
                    };
            }
        }
        public override List<ListItemInfo> GetListItems(string listId){
            return new List<ListItemInfo>{
                new ListItemInfo(
                    "current", 
                    "Current", 
                    "Gets the current stage number",
                    ListItemInfoType.None),
                new ListItemInfo(
                    "remain",
                    "Remain",
                    "Gets the number of remain stages",
                    ListItemInfoType.None)
            };
        }
        public override void SetListValue(string listId, string value){
            _type = value;
            OnTypeChanged();
            Debug.Log($"listId {listId}, value {value}");
        }
        public override string GetListValue(string listId){
            return _type;
        }
        private void OnTypeChanged(){
            switch(_type){
                case("current"):
                    _infoType = StagingInformationType.Current;
                    break;
                case("remain"):
                    _infoType = StagingInformationType.Remain;
                    break;
                case("parts"):
                    _infoType = StagingInformationType.Parts;
                    break;
            }
        }
        public override void OnDeserialized(XElement xml) {
            base.OnDeserialized(xml);
            this.OnTypeChanged();
        }
    }
    public enum StagingInformationType{
        Current = 1,
        Remain,
        Parts
    }
}