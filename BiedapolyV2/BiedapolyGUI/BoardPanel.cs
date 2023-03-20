using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Biedapoly;


namespace BiedapolyGUI
{
    public class BoardPanel
    {
        List<RefreshField> fields = new List<RefreshField>();

        public List<RefreshField> Fields { get => fields; set => fields = value; }

       

        public BoardPanel() 
        {
            AddAllElement();
        }

        private void AddAllElement() 
        {
            Game game = MainWindow.GetMyGame();
            
            Canvas canvasStart = new Canvas();
            StartFieldElement start = new StartFieldElement(canvasStart, game.Map.DictionaryOfTerritories["000"],Location.South);
            Fields.Add(start);
            
            
            Canvas canvasKonopacka= new Canvas();
            StreetFieldScreenElement ulKonopacka = new StreetFieldScreenElement(canvasKonopacka, game.Map.DictionaryOfTerritories["001"], Location.South);
            Fields.Add(ulKonopacka);


            Canvas canvasKasaSpoleczna1 = new Canvas();
            FieldScreenElement KasaSpoleczna = new FieldScreenElement(canvasKasaSpoleczna1, game.Map.DictionaryOfTerritories["002"], Location.South);
            Fields.Add(KasaSpoleczna);



            Canvas canvasUlStalowa = new Canvas();
            StreetFieldScreenElement ulStalowa = new StreetFieldScreenElement(canvasUlStalowa, game.Map.DictionaryOfTerritories["003"], Location.South);
            Fields.Add(ulStalowa);



            Canvas canvasPodatekDochodowy = new Canvas();
            FieldScreenElement PodatekDochodowy = new FieldScreenElement(canvasPodatekDochodowy, game.Map.DictionaryOfTerritories["004"], Location.South);
            Fields.Add(PodatekDochodowy);


            Canvas canvasDworzecGdański = new Canvas();
            FieldScreenElement DworzecGdański = new FieldScreenElement(canvasDworzecGdański, game.Map.DictionaryOfTerritories["005"], Location.South);
            Fields.Add(DworzecGdański);


            Canvas canvasUlRadzyminska = new Canvas();
            StreetFieldScreenElement ULRadzyminska = new StreetFieldScreenElement(canvasUlRadzyminska, game.Map.DictionaryOfTerritories["006"], Location.South);
            Fields.Add(ULRadzyminska);


            Canvas CanvasSzansa = new Canvas();
            FieldScreenElement Szansa = new FieldScreenElement(CanvasSzansa, game.Map.DictionaryOfTerritories["007"], Location.South);
            Fields.Add(Szansa);




            Canvas canvasUlJagiellonska = new Canvas();
            StreetFieldScreenElement UlJagiellonska = new StreetFieldScreenElement(canvasUlJagiellonska, game.Map.DictionaryOfTerritories["008"], Location.South);
            Fields.Add(UlJagiellonska);



            Canvas canvasUlTargowa= new Canvas();
            StreetFieldScreenElement ULTargowa = new StreetFieldScreenElement(canvasUlTargowa, game.Map.DictionaryOfTerritories["009"], Location.South);
            Fields.Add(ULTargowa);


            Canvas CanvasWiezienie = new Canvas();
            JailFieldScreenElement Wiezienie = new JailFieldScreenElement(CanvasWiezienie, game.Map.DictionaryOfTerritories["010"], Location.South);
            Fields.Add(Wiezienie);


            Canvas canvasUlPlowiecka = new Canvas();
            StreetFieldScreenElement UlPlowiecka = new StreetFieldScreenElement(canvasUlPlowiecka, game.Map.DictionaryOfTerritories["011"], Location.West);
            Fields.Add(UlPlowiecka);

            Canvas CanvasElektrownia = new Canvas();
            FieldScreenElement Elektrownia = new FieldScreenElement(CanvasElektrownia, game.Map.DictionaryOfTerritories["012"], Location.West);
            Fields.Add(Elektrownia);


            Canvas canvasUlMarsa = new Canvas();
            StreetFieldScreenElement UlMarsa = new StreetFieldScreenElement(canvasUlMarsa, game.Map.DictionaryOfTerritories["013"], Location.West);
            Fields.Add(UlMarsa);

            Canvas canvasUlGrochowska = new Canvas();
         
            StreetFieldScreenElement UlGrochowska = new StreetFieldScreenElement(canvasUlGrochowska, game.Map.DictionaryOfTerritories["014"], Location.West);
            Fields.Add(UlGrochowska);



            Canvas canvasDworzecZachodni = new Canvas();
            FieldScreenElement DworzecZachodni = new FieldScreenElement(canvasDworzecZachodni, game.Map.DictionaryOfTerritories["015"], Location.West);
            Fields.Add(DworzecZachodni);


            Canvas CanvasUlObozowa = new Canvas();
            StreetFieldScreenElement UlObozowa = new StreetFieldScreenElement(CanvasUlObozowa, game.Map.DictionaryOfTerritories["016"], Location.West);
            Fields.Add(UlObozowa);


            Canvas CanvasKasaSpoleczna = new Canvas();
            FieldScreenElement KasaSpoleczna2 = new FieldScreenElement(CanvasKasaSpoleczna, game.Map.DictionaryOfTerritories["017"], Location.West);
            Fields.Add(KasaSpoleczna2);




            Canvas canvasUlGorczewska = new Canvas();
            StreetFieldScreenElement UlGorczewska = new StreetFieldScreenElement(canvasUlGorczewska, game.Map.DictionaryOfTerritories["018"], Location.West);
            Fields.Add(UlGorczewska);



            Canvas canvasUlWolska = new Canvas();
            StreetFieldScreenElement UlWolska = new StreetFieldScreenElement(canvasUlWolska, game.Map.DictionaryOfTerritories["019"], Location.West);
            Fields.Add(UlWolska);


            Canvas CanvasParking = new Canvas();
            FieldScreenElement Parking = new FieldScreenElement(CanvasParking, game.Map.DictionaryOfTerritories["020"],Location.West);
            Fields.Add(Parking);


            Canvas CanvasUlMickiewicza = new Canvas();
            StreetFieldScreenElement UlMickiewicza = new StreetFieldScreenElement(CanvasUlMickiewicza, game.Map.DictionaryOfTerritories["021"], Location.North);
            Fields.Add(UlMickiewicza);


            Canvas CanvasSzansa2 = new Canvas();
            FieldScreenElement Szansa2 = new FieldScreenElement(CanvasSzansa2, game.Map.DictionaryOfTerritories["022"], Location.North);
            Fields.Add(Szansa2);




            Canvas CanvasUlSlowackiego = new Canvas();
            StreetFieldScreenElement UlSlowackiego = new StreetFieldScreenElement(CanvasUlSlowackiego, game.Map.DictionaryOfTerritories["023"], Location.North);
            Fields.Add(UlSlowackiego);



            Canvas canvasPlacWilsona = new Canvas();
            StreetFieldScreenElement PlacWilsona = new StreetFieldScreenElement(canvasPlacWilsona, game.Map.DictionaryOfTerritories["024"], Location.North);
            Fields.Add(PlacWilsona);





            Canvas canvasDworzecWschodni = new Canvas();
            FieldScreenElement DworzecWschodni = new FieldScreenElement(canvasDworzecWschodni, game.Map.DictionaryOfTerritories["025"], Location.North);
            Fields.Add(DworzecWschodni);





            Canvas CanvasUlSwietokrzyska = new Canvas();
            StreetFieldScreenElement UlSwetokrzyska = new StreetFieldScreenElement(CanvasUlSwietokrzyska, game.Map.DictionaryOfTerritories["026"], Location.North);
            Fields.Add(UlSwetokrzyska);



            Canvas CanvasKrakowskiePrzedmiescie = new Canvas();
            StreetFieldScreenElement UlKrakowskiePrzedmiescie = new StreetFieldScreenElement(CanvasKrakowskiePrzedmiescie, game.Map.DictionaryOfTerritories["027"], Location.North);
            Fields.Add(UlKrakowskiePrzedmiescie);



            Canvas CanvasWodociagi = new Canvas();
            FieldScreenElement Wodociagi = new FieldScreenElement(CanvasWodociagi, game.Map.DictionaryOfTerritories["028"], Location.North);
            Fields.Add(Wodociagi);




      
            Canvas CanvasNowySwiat = new Canvas();
            StreetFieldScreenElement NowySwiat = new StreetFieldScreenElement(CanvasNowySwiat, game.Map.DictionaryOfTerritories["029"], Location.North);
            Fields.Add(NowySwiat);


            Canvas CanvasIdzDoWiezienia = new Canvas();
            FieldScreenElement IdzDoWiezienia = new FieldScreenElement(CanvasIdzDoWiezienia, game.Map.DictionaryOfTerritories["030"], Location.North);
            Fields.Add(IdzDoWiezienia);


            Canvas CanvasPlacTrzechKrzyzy= new Canvas();
            StreetFieldScreenElement PlacTrzechKrzyzy = new StreetFieldScreenElement(CanvasPlacTrzechKrzyzy, game.Map.DictionaryOfTerritories["031"], Location.East);
            Fields.Add(PlacTrzechKrzyzy);



            Canvas CanvasUlMarszalkowska = new Canvas();
            StreetFieldScreenElement UlMarszalkowska = new StreetFieldScreenElement(CanvasUlMarszalkowska, game.Map.DictionaryOfTerritories["032"], Location.East);
            Fields.Add(UlMarszalkowska);



            Canvas CanvasKasaSpoleczna3 = new Canvas();
            FieldScreenElement KasaSpoleczna3 = new FieldScreenElement(CanvasKasaSpoleczna3, game.Map.DictionaryOfTerritories["033"], Location.East);
            Fields.Add(KasaSpoleczna3);





            Canvas CanvasAlejeJerozolimskie = new Canvas();
            StreetFieldScreenElement AlejeJerozolimskie = new StreetFieldScreenElement(CanvasAlejeJerozolimskie, game.Map.DictionaryOfTerritories["034"], Location.East);
            Fields.Add(AlejeJerozolimskie);



            Canvas CanvasDworzecCentralny = new Canvas();
            FieldScreenElement DworzecCentralny = new FieldScreenElement(CanvasDworzecCentralny, game.Map.DictionaryOfTerritories["035"], Location.East);
            Fields.Add(DworzecCentralny);





            Canvas CanvasSzansa3 = new Canvas();
            FieldScreenElement Szansa3 = new FieldScreenElement(CanvasSzansa3, game.Map.DictionaryOfTerritories["036"], Location.East);
            Fields.Add(Szansa3);




            Canvas CanvasUlicaBelwederska = new Canvas();
            StreetFieldScreenElement UlicaBelwederska = new StreetFieldScreenElement(CanvasUlicaBelwederska, game.Map.DictionaryOfTerritories["037"], Location.East);
            Fields.Add(UlicaBelwederska);





            Canvas CanvasPodatekDochody = new Canvas();
            FieldScreenElement PodatekDochodowy2 = new FieldScreenElement(CanvasPodatekDochody, game.Map.DictionaryOfTerritories["038"], Location.East);
            Fields.Add(PodatekDochodowy2);





            Canvas CanvasAlejeUjazdowskie = new Canvas();
            StreetFieldScreenElement AlejeUjazdowskie = new StreetFieldScreenElement(CanvasAlejeUjazdowskie, game.Map.DictionaryOfTerritories["039"], Location.East);
            Fields.Add(AlejeUjazdowskie);


        }


        public void GroupedRefresh() 
        {
            foreach(var item in Fields) 
            {
                item.Refresh();
            }
        }



    }
}
