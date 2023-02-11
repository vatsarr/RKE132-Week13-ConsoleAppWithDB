using System.Data.SQLite;

//CreateConnection();
//ReadData(CreateConnection());
//InsertNewCustomer(CreateConnection());
//DeleteCustomerByID(CreateConnection());
FindCustomer(CreateConnection());

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source = mydb.db; Version = 3; New = True; Compress = True");

    try
    {
        connection.Open();
        Console.WriteLine("DB found!");
    }
    catch
    {
        Console.WriteLine("DB not found");
    }
    return connection;
}

static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText =
      "SELECT rowid, * FROM customer ";

    //"SELECT customer.firstname, customer.lastname, status.statustype FROM customerStatus " +
    //"JOIN customer on customerStatus.customerid = customer.rowid " +
    //"JOIN status on customerStatus.statusid = status.rowid " +
    //"ORDER BY status.statustype ";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFirstName} {readerStringLastName}; Status: {readerStringDoB}");
    }
    myConnection.Close();
}

static void InsertNewCustomer(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter DoB (mm-dd-yyyy):");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText =
        $"INSERT INTO customer (firstname, lastname, dateofbirth) " +
        $"VALUES ('{fName}','{lName}','{dob}') ";
    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");

    ReadData(myConnection);
}

static void DeleteCustomerByID(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteCommand command;
    string customerID;
    Console.WriteLine("Enter ID to delete customer: ");
    customerID = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText =
        $"DELETE FROM customer " +
        $"WHERE rowid = {customerID} ";

    int rowDeleted = command.ExecuteNonQuery();
    Console.WriteLine($"{rowDeleted} was deleted from the table Customer!");

    ReadData(myConnection);
}

static void FindCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;

    Console.WriteLine("Enter first name of a customer:");
    searchName = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText =
        $"SELECT customer.rowid, customer.firstname, customer.lastname, status.statustype " +
        $"FROM customerstatus " +
        $"JOIN customer ON customer.rowid = customerstatus.customerid " +
        $"JOIN status ON status.rowid = customerstatus.statusid " +
        $"WHERE firstname LIKE '{searchName}'";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString();
        string readerFirstName = reader.GetString(1);
        string readerLastName = reader.GetString(2);
        string readerStatus = reader.GetString(3);

        Console.WriteLine($"Search result for {searchName}:");
        Console.WriteLine($"ID: {readerRowId}. Name: {readerFirstName} {readerLastName}. Status: {readerStatus}.");
    }

    myConnection.Close();
}