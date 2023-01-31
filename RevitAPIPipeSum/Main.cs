using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIPipeSum
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберете элементы");
            var pipeList = new List<double>();
            double sumLength = 0;
            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe pipe = doc.GetElement(selectedElement) as Pipe;
                Parameter length =pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                double lengthValue = UnitUtils.ConvertFromInternalUnits(length.AsDouble(), DisplayUnitType.DUT_METERS);
                sumLength += lengthValue;
            }
            TaskDialog.Show("Длина труб", sumLength.ToString());

            return Result.Succeeded;
        }
    }
}
