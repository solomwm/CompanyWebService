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
        private string _dataFilePath = String.Empty;
        private int _currentId = 1;

        public CompanyesDataEngine()
        {
            formatter = new BinaryFormatter();
            companyes = new List<Company>();
            _dataFilePath = Directory.GetCurrentDirectory() + @"\wwwroot\CompanyesDB.dat";
        }

        public CompanyesDataEngine(string dataFilePath): this()
        {
            _dataFilePath = dataFilePath;
            using (FileStream stream = new FileStream(dataFilePath, FileMode.OpenOrCreate))
            {
                try
                {
                    companyes = formatter.Deserialize(stream) as List<Company>;
                    _currentId = getLastId() + 1;
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
        public int CurrentId { get => _currentId; }

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
                using (FileStream stream = new FileStream(_dataFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    formatter.Serialize(stream, companyes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void EditOrCreate(Company company)
        {
            var cmp = companyes.FirstOrDefault((c) => c.Id == company.Id);
            if (cmp != null)
            {
                cmp.INN = company.INN;
                cmp.Name = company.Name;
            }
            else
            {
                company.Id = _currentId++;
                companyes.Add(company);
            }
            SaveData();
        }

        public void Delete(int id)
        {
                var cmp = companyes.FirstOrDefault((c) => c.Id == id);
                if (cmp != null)
                {
                    companyes.Remove(cmp);
                    SaveData();
                }      
        }

        private int getLastId()
        {
            return companyes.Max((c) => c.Id);
        }
    }
}