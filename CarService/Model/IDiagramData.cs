using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Model
{
    public interface IDiagramData
    {
        List<KeyValuePair<string, int>> GetDataForDiagramCarBrand();
        List<KeyValuePair<string, int>> GetDataForDiagramMonth();
        List<KeyValuePair<string, int>> GetDataForDiagramPrice(ref List<int> values);
    }
}
