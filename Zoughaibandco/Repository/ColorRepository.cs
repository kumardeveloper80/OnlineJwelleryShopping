using System.Linq;
using Zoughaibandco.Models;

namespace Zoughaibandco.Repository
{
    public class ColorRepository
    {
        Zoughaibandco_DBEntities _DBContext;

        public ColorRepository()
        {
            _DBContext = new Zoughaibandco_DBEntities();
        }

        public void AddColor(int ProductId, string[] ColorList)
        {
            var oldColors = _DBContext.Colors.Where(x => x.ProductId == ProductId).ToList();
            if (oldColors.Count > 0)
            {
                _DBContext.Colors.RemoveRange(oldColors);
                _DBContext.SaveChanges();
            }

            if (ColorList[0] != "undefined")
            {
                Color colorObj = new Color();
                foreach (var color in ColorList)
                {
                    if (color.Trim() != "")
                    {
                        colorObj.ProductId = ProductId;
                        colorObj.ColorsName = "#" + color;
                        _DBContext.Colors.Add(colorObj);
                        _DBContext.SaveChanges();
                    }
                }
            }
        }
    }
}