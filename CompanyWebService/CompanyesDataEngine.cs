using INNService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace INNService
{
    public class CompanyesDataEngine
    {
        private List<Company> companyes;
        private BinaryFormatter formatter;
        private string _dataFilePath;

        public CompanyesDataEngine(string dataFilePath)
        {
            formatter = new BinaryFormatter();
            _dataFilePath = dataFilePath;
            using (FileStream stream = new FileStream(dataFilePath, FileMode.OpenOrCreate))
            {
                try
                {
                    companyes = formatter.Deserialize(stream) as List<Company>;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        ~CompanyesDataEngine()
        {
            SaveData();
        }

        public List<Company> Companyes
        {
            get => companyes;
            set => companyes = value;
        }

        public Company GetCompany(int id)
        {
            return companyes.FirstOrDefault((c) => c.Id == id);
        }

        public Company GetByInn(string inn)
        {
            return companyes.FirstOrDefault((c) => c.INN.Equals(inn));
        }

        public IEnumerable<Company> GetByName(string name)
        {
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            return companyes.FindAll((c) => c.Name.IndexOf(name, comp) >= 0);
        }

        public void SaveData()
        {
            try
            {
                using (FileStream stream = new FileStream(_dataFilePath, FileMode.Open, FileAccess.Write))
                {
                    formatter.Serialize(stream, companyes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}