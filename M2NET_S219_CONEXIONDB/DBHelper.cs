using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace M2NET_S219_CONEXIONDB
{
    class DBHelper
    {
        private SqlConnection cadConexion;
        private string server;
        private string user;
        private string pass;
        private string bd;
        private SqlDataAdapter maAdapter;
        private DataSet ds;

        public DBHelper(string server, string user, string pass, string bd)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.bd = bd;
            cadConexion = new SqlConnection("server=" + server + "\\SQLEXPRESS; DataBase=" + bd
                + "; User ID=" + user + "; pwd=" + pass);
        }

        public void AbrirConexion()
        {
            try
            {
                cadConexion.Open();
                Console.WriteLine("Conexion abierta ...");
            }
            catch (Exception e)
            {

                Console.WriteLine("Error al intentar abrir la conexion error: "+e.Message);
            }
        }
        public void CerrarConexion()
        {
            try
            {
                cadConexion.Close();
                Console.WriteLine("Conexion cerrada ...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar cerrar la conexion error: " + e.Message);
            }
        }
        //conectado
        public List<Clientes> mostrarClientes()
        {
            List<Clientes> lstClientes = new List<Clientes>();
            Clientes clienteSelected;
            try
            {
                this.AbrirConexion();
                SqlCommand cmdCliente = cadConexion.CreateCommand();
                cmdCliente.CommandText = "select c.CustomerID,c.CompanyName,c.ContactName " +
                    "from Customers c";
                SqlDataReader dr = cmdCliente.ExecuteReader();
                while (dr.Read())
                {
                    clienteSelected = new Clientes()
                    {
                        Id = dr["CustomerID"].ToString(),
                        Empresa = dr["CompanyName"].ToString(),
                        Contacto = dr["ContactName"].ToString()
                    };
                    lstClientes.Add(clienteSelected);
                }
                dr.Close();
                this.CerrarConexion();              
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo mostrar informacion: "+e.Message);
            }
            return lstClientes;
        }

        //desconectado
        public List<Clientes> mostrarClientesDS()
        {
            ds = new DataSet();
            List<Clientes> lstClientes = new List<Clientes>();
            Clientes clienteSelected;
            try
            {
                maAdapter = new SqlDataAdapter("select c.CustomerID,c.CompanyName,c.ContactName " +
                    "from Customers c", cadConexion);
                maAdapter.Fill(ds, "Customers");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    clienteSelected = new Clientes()
                    {
                        Id = ds.Tables[0].Rows[i]["CustomerID"].ToString(),
                        Empresa = ds.Tables[0].Rows[i]["CompanyName"].ToString(),
                        Contacto = ds.Tables[0].Rows[i]["ContactName"].ToString()
                    };
                    lstClientes.Add(clienteSelected);
                }
                maAdapter.Dispose();
                ds.Dispose();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error de forma desconectada "+e.Message);
            }
            return lstClientes;
        }

        //insertar datos con SqlCommandBuilder.
        public void Insertar()
        {
            ds = new DataSet();
            this.AbrirConexion();
            maAdapter = new SqlDataAdapter("select * from region", cadConexion);
            maAdapter.Fill(ds);
            SqlCommandBuilder builder = new SqlCommandBuilder(maAdapter);
            SqlCommand insertar = builder.GetInsertCommand();
            insertar.Parameters["@p1"].Value = ds.Tables[0].Rows.Count + 1;
            insertar.Parameters["@p2"].Value = "Oriente";
            insertar.ExecuteNonQuery();
            maAdapter.Dispose();
            ds.Dispose();
            this.CerrarConexion();
        }
        //Elimanar usando SqlcommandBuilder
        public void Eliminar()
        {
            ds = new DataSet();
            SqlCommand command = new SqlCommand
            ("SELECT * FROM Region", cadConexion);
            maAdapter.SelectCommand = command;
            maAdapter.Fill(ds);
            SqlCommandBuilder builder = new SqlCommandBuilder(maAdapter);
            SqlCommand delete = new SqlCommand("DELETE FROM Region WHERE ID = @ID",
                cadConexion);
            delete.Parameters.Add("@ID", SqlDbType.Int).Value = ds.Tables[0].Rows.Count;
            cadConexion.Open();
            delete.ExecuteNonQuery();
            cadConexion.Close();
           //maAdapter.Fill(ds);
        }

    }
}
