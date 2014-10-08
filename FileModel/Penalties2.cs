using System.Collections.Generic;
using System.Linq;
using PASaveEditor;

namespace FileModel {
    internal class Penalties2 : Node {
        public readonly Dictionary<int, List<Penalty>> Penalties = new Dictionary<int, List<Penalty>>();


        public Penalties2(string label)
            : base(label, true) {}


        public override void ReadKey(string key, string value) {
            if (!key.Equals("Size")) {
                base.ReadKey(key, value);
            }
        }


        public override Node CreateNode(string label) {
            if (Parser.IsId(label)) {
                return new Penalty(label);
            } else {
                return base.CreateNode(label);
            }
        }


        public override void FinishedReadingNode(Node node) {
            var penaltyNode = node as Penalty;
            if (penaltyNode != null) {
                List<Penalty> list;
                if (!Penalties.TryGetValue(penaltyNode.ObjectId, out list)) {
                    list = new List<Penalty>();
                    Penalties.Add(penaltyNode.ObjectId, list);
                }
                list.Add(penaltyNode);
            }
        }


        public override void WriteProperties(Writer writer) {
            writer.WriteProperty("Size", Penalties.Values.Sum(penList => penList.Count));
        }


        public override void WriteNodes(Writer writer) {
            foreach (var prisoner in Penalties) {
                foreach (Penalty penalty in prisoner.Value) {
                    writer.WriteNode(penalty);
                }
            }
        }
    }
}