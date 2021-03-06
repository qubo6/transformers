﻿using Data.Enum;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SectionRepository : ConnectionManger
    {
        public bool InsertSection(ModelSection modelSection)
        {
            bool success = false;

            Execute((command) =>
            {
                command.CommandText = @"Insert into section ([Name], [Code], [HierarchyLevel], [ParentSectionID])
                                    Values(@Name, @Code, @Hierarchy, @Parent)";
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = modelSection.Name;
                command.Parameters.Add("@Code", SqlDbType.VarChar).Value = modelSection.Code;
                command.Parameters.Add("@Hierarchy", SqlDbType.Int).Value = modelSection.HierarchyLevel;
                command.Parameters.Add("@Parent", SqlDbType.Int).Value = (object)modelSection.ParentSectionId ?? DBNull.Value;
                if (command.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            });
            return success;
        }

        public bool UpdateDirectorIdOfSection(ModelSection modelSection)
        {
            bool success = false;
            Execute((command) =>
            {
                command.CommandText = "Update section SET DirectorId=@idDirector where id=@idSection";
                command.Parameters.Add("@idDirector", SqlDbType.Int).Value = modelSection.DirectorId;
                command.Parameters.Add("@idSection", SqlDbType.Int).Value = modelSection.Id;
                if (command.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            });
            return success;
        }

        public bool UpdateDirectorIdOfSectionToNull(int directorId)
        {
            bool success = false;
            Execute((command) =>
            {
                command.CommandText = "Update section SET DirectorId=null where DirectorId=@idDirector";
                command.Parameters.Add("@idDirector", SqlDbType.Int).Value = directorId;
                if (command.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            });
            return success;
        }

        public List<ModelSection> GetAllCompanies()
        {
            List<ModelSection> sections = new List<ModelSection>();
            Execute((command) =>
            {
                command.CommandText = "Select * from section where ParentSectionID is null";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int sectionId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string code = reader.GetString(2);
                        int? directorId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3);
                        HierarchyLevel level = (HierarchyLevel)reader.GetInt32(4);
                        int? idParent = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5);
                        sections.Add(new ModelSection(sectionId, name, code, directorId, level, idParent));
                    }
                }
            });
            return sections;
        }

        public List<ModelSection> GetSectionsByParentId(int? parentId)
        {
            List<ModelSection> sections = new List<ModelSection>();
            Execute((command) =>
            {
                command.CommandText = "Select * from section where ParentSectionID=@parentId";
                command.Parameters.Add("@parentId", SqlDbType.Int).Value = (object)parentId ?? DBNull.Value;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int sectionId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string code = reader.GetString(2);
                        int? directorId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3);
                        HierarchyLevel level = (HierarchyLevel)reader.GetInt32(4);
                        int? idParent = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5);
                        sections.Add(new ModelSection(sectionId, name, code, directorId, level, idParent));
                    }
                }
            });
            return sections;
        }
        public bool UpdateSection(ModelSection modelSection)
        {
            bool success = false;
            Execute((command) =>
            {
                command.CommandText = "Update section set name = @name, code = @code where id=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = modelSection.Id;
                command.Parameters.Add("@name", SqlDbType.VarChar).Value = modelSection.Name;
                command.Parameters.Add("@code", SqlDbType.VarChar).Value = modelSection.Code;
                if (command.ExecuteNonQuery() > 0)
                {
                    success = true;
                }
            });
            return success;
        }
        public ModelSection SelectSectionById(int sectionId)
        {
            ModelSection selectedSection = new ModelSection();
            Execute((command) =>
            {
                command.CommandText = "select name, code from section where id=@idSection";
                command.Parameters.Add("@idSection", SqlDbType.Int).Value = selectedSection.Id;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        selectedSection.Name= reader.GetString(0);
                        selectedSection.Code= reader.GetString(1);
                    }
                }
            });
            return selectedSection;
        }

    }
}
