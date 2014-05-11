using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zolilo.Data
{
    public class Tag2Goal_GoalTag : Tag2GoalEdge
    {
        public static long edgeType = edgeType == 0 ? Tag2GoalEdge.RegisterEdgeType(typeof(Tag2Goal_GoalTag), Tag2GoalEdgeType.GoalTag) : edgeType;

        public override long EdgeType
        {
            get { return edgeType; }
        }

        public override Control GetContextControl()
        {
            PlaceHolder ph = new PlaceHolder();
            ph.Controls.Add(new LiteralControl("<a href=\"/vertex/view?id=" + ID.ToString() + "\">View Connection</a><br>"));
            ph.Controls.Add(new LiteralControl("<a href=\"/vertex/delete?id=" + ID.ToString()+ "\">Delete Connection</a><br>"));


            return ph;
        }
    }
}
