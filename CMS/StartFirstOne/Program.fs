open System
open System.Data
open Microsoft.Data.SqlClient
open MySql.Data.MySqlClient


// start make connection with SQL database
let retrieveData () =
    // connection data 
    let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
    printfn "connection start..."
    try
        use connection = new MySqlConnection(connectionString)
        connection.Open()
        printfn "Connection successful"
        let query = "SELECT Id, Name, Age FROM SampleTable"
        use command = new MySqlCommand(query, connection)
        use reader = command.ExecuteReader()
        while reader.Read() do
            let id = reader.GetInt32(0)
            let name = reader.GetString(1)
            let age = reader.GetInt32(2)
            printfn "Id: %d, Name: %s, Age: %d" id name age
    with
    | ex -> printfn "Error: %s" ex.Message

retrieveData ()

printfn "Data Selected successfully!"

printfn "Hello from F#"