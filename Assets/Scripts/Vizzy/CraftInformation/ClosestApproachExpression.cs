using System;
using System.Collections.Generic;
using ModApi.Craft.Program;
using ModApi.Flight.Sim;
using UnityEngine;

namespace Assets.Scripts.Vizzy.CraftInformation {
    [Serializable]
    public class ClosestApproachExpression : ProgramExpression {
        public const String XmlName = "ClosestApproach";

        [ProgramNodeProperty]
        private string _type;

        public override bool IsBoolean => false;

        public override List<ListItemInfo> GetListItems(string listId) {
            return new List<ListItemInfo> {
                new ListItemInfo(
                    "time-of",
                    "Time of",
                    "Time of next Closest Approach.",
                    ListItemInfoType.Number),
                new ListItemInfo(
                    "distance-at",
                    "Distance at",
                    "Distance at next Closest Approach.",
                    ListItemInfoType.Number)
            };
        }

        public override string GetListValue(string listId) {
            return this._type;
        }

        public override string SetListValue(string listId, string value) {
            this._type = value;
        }

        public override void OnDeserialized(XElement xml) {
            base.OnDeserialized(xml);
        }

        private ExpressionResult GetExpressionResult(Int32 TargetCraftId) {
            if (this._type == "time-of") {
                return new ExpressionResult {};
            } else if (this._type == "distance-at") {
                return new ExpressionResult {};
            } else {
                Debug.Log("Target Craft not set");
                return new ExpressionResult {};
            }
        }
    }
}
