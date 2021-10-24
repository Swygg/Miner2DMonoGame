namespace MinerLogic.Models
{
    public class MapElements
    {
        public int NbLines { get; set; }
        public int NbRows { get; set; }
        public int NbMines { get; set; }

        public MapElements(int nbLines, int nbRow, int nbMines)
        {
            this.NbLines = nbLines;
            this.NbRows = nbRow;
            this.NbMines = nbMines;
        }
    }
}
