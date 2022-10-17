using SldWorks;
using SwConst;
using System;
using System.Runtime.InteropServices;

namespace SketchExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SldWorks.SldWorks swApp = null;

            #region Get or Create SolidWorks Application
            try
            {
                swApp = Marshal.GetActiveObject("SldWorks.Application") as SldWorks.SldWorks;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147221021)
                {
                    swApp = Activator.CreateInstance(Type.GetTypeFromProgID("SldWorks.Application")) as SldWorks.SldWorks;
                }
            }
            #endregion

            swApp.Visible = true;
            swApp.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swInputDimValOnCreate, false);


            //Create a New Part - Criar uma nova peça
            ModelDoc2 swModel = swApp.NewPart();



            //Create Sketch in Top Plane - Cria um Sketch no plano superior
            _ = swModel.Extension.SelectByID2("Plano Superior", "PLANE", 0, 0, 0, false, 0, null, 0);//Select Top Plane
            swModel.SketchManager.InsertSketch(true);//Insert Sketch
            Sketch swSketch;
            //Guarde o Sketch em uma variável
            swSketch = swModel.GetActiveSketch2();//Keep this Sketch in the variable
            Feature swFeat;
            swFeat = (Feature)swSketch;
            swFeat.Name = "New_Sketch";// Rename the Sketch - Renomeia o Sketch



            //Insira o valor do raio em metros
            double radius = 0.01; // Set a value for the radius in meters
            //Ponto em x,y e z onde o círculo será inserido
            double[] Lpoint = new double[3];//Point in x,y and z where the circle will be inserted
            Lpoint[0] = -0.2;   // X distance in Meter - Distância X em metros
            Lpoint[1] = 0.06;   // Y distance in Meter - Distância Y em metros
            Lpoint[2] = 0;      // Z distance in Meter - Distância Z em metros


            SketchArc skArc;
            // Inserts the circle at the established position and radius
            // Insere o círculo na posição e raios estabelecidos
            skArc = (SketchArc)swModel.SketchManager.CreateCircle(Lpoint[0], Lpoint[1], Lpoint[2], Lpoint[0], Lpoint[1] - radius, Lpoint[2]);

            //Get All Segments from Sketch - Pega todos os segmentos do Sketch
            var vArcs = swSketch.GetSketchSegments();

            //Guarda o nome do círculo criado
            string Arc1 = "Arc" + vArcs[0].GetID()[1]; //Get name for Created Arc



            SketchPoint skPoint;
            skPoint = (SketchPoint)skArc.GetCenterPoint2();
            string Point1, Point2;
            //Guarda o nome do ponto central do círculo
            Point1 = "Point" + skPoint.GetID()[1]; //Get the name of the center point of the arc



            Dimension myDimension;
            DisplayDimension myDisplayDim;

            swModel.ClearSelection2(true);
            //Select the Arc1 - Seleciona o Arc1
            _ = swModel.Extension.SelectByID2(Arc1, "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            //Set the Arc1 diameter - Define o diâmetro do Arc1
            myDisplayDim = swModel.AddDimension2(Lpoint[0] - .1, Lpoint[1], Lpoint[2]);




            swModel.ClearSelection2(true);
            //Select the center point of the arc - Seleciona o ponto central do círculo
            _ = swModel.Extension.SelectByID2(Point1, "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            //Adds the Right Plane to the selection - Adiciona o Plano direito à seleção
            //"Right Plane"
            _ = swModel.Extension.SelectByID2("Plano Direito", "PLANE", 0, 0, 0, true, 0, null, 0);
            //Adds a dimension between both - Adiciona uma dimensão entre ambos
            _ = swModel.AddDimension2(Lpoint[0] / 2, Lpoint[2], Lpoint[1]);



            swModel.ClearSelection2(true);
            //Select the center point of the Arc1 - Seleciona o ponto central do Arc1
            _ = swModel.Extension.SelectByID2(Point1, "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            //Adds the Front Plane to the selection - Adiciona o Plano Frontal à seleção
            //"Front Plane"
            _ = swModel.Extension.SelectByID2("Plano Frontal", "PLANE", 0, 0, 0, true, 0, null, 0);
            //Adds a dimension between both - Adiciona uma dimensão entre ambos
            _ = swModel.AddDimension2(Lpoint[0] / 2, Lpoint[0], Lpoint[1] / 2 * -1);



            // For the second Arc - Para o segundo círculo
            Lpoint[0] = 0.2;// X distance in Meter - Distância X em metros
            Lpoint[1] = 0.06;// Y distance in Meter - Distância Y em metros
            Lpoint[2] = 0;// Z distance in Meter - Distância Z em metros

            // Inserts the second circle at the established position and radiu
            // Insere o segundo círculo na posição e com o raio estabelecidos
            skArc = (SketchArc)swModel.SketchManager.CreateCircle(Lpoint[0], Lpoint[1], Lpoint[2], Lpoint[0], Lpoint[1] - radius, Lpoint[2]);
            //Guarda o nome do círculo criado
            skPoint = (SketchPoint)skArc.GetCenterPoint2();
            Point2 = "Point" + skPoint.GetID()[1]; //Get name for Created Arc

            //Get All Segments from Sketch - Pega todos os segmentos do Sketch
            vArcs = swSketch.GetSketchSegments();

            //Guarda o nome do círculo criado
            string Arc2 = "Arc" + vArcs[1].GetID()[1]; //Get name for Created Arc

            swModel.ClearSelection2(true);
            //Select the center point of the Arc1 - Seleciona o ponto central do Arc1
            _ = swModel.Extension.SelectByID2(Point1, "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            //Adds the center point of the Arc2 to selection - Adiciona o ponto central do Arc2 à seleção
            _ = swModel.Extension.SelectByID2(Point2, "SKETCHPOINT", 0, 0, 0, true, 0, null, 0);
            //Inserts a horizontal alignment relationship between both
            //Insere uma relação de alinhamento horizontal entre ambos
            swModel.SketchAddConstraints("sgHORIZONTALPOINTS2D");



            swModel.ClearSelection2(true);
            //Select the first circle - Seleciona o primeiro círculo
            _ = swModel.Extension.SelectByID2(Arc1, "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            //Select the second circle - Seleciona o segundo círculo
            _ = swModel.Extension.SelectByID2(Arc2, "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            //Inserts a relationship of equality between both
            //Insere uma relação de igualdade entre ambos
            swModel.SketchAddConstraints("sgSAMELENGTH");



            swModel.ClearSelection2(true);
            //Select the center point of the Arc2 - Seleciona o ponto central do Arc2
            _ = swModel.Extension.SelectByID2(Point2, "SKETCHPOINT", 0, 0, 0, false, 0, null, 0);
            //Adds the Rigth Plane to the selection - Adiciona o Plano Direito à seleção
            //"Right Plane"
            _ = swModel.Extension.SelectByID2("Plano Direito", "PLANE", 0, 0, 0, true, 0, null, 0);
            //Adds a dimension between both - Adiciona uma dimensão entre ambos
            _ = swModel.AddDimension2(Lpoint[0] / 2, Lpoint[2], Lpoint[1]);



            swModel.ClearSelection2(true);
            //Selects the diameter dimension of Arc1 - Seleciona a dimensão de diametro do Arc1
            myDimension = swModel.Parameter(myDisplayDim.GetNameForSelection());
            //Change the dimension of Arc1 for 40mm - Muda a dimensão de Arc1 para 40mm
            myDimension.SystemValue = 0.04;
            swModel.SketchManager.InsertSketch(true);
            swModel.ClearSelection2(true);
        }
    }
}
