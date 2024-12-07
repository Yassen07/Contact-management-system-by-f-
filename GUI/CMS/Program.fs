open System
open System.Windows.Forms
open Microsoft.Data.SqlClient
open MySql.Data.MySqlClient

// the address of the connection
// start make connection with SQL database
let retrieveData () =
    // connection data 
    let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
    printfn "connection start..."
    try
        use connection = new MySqlConnection(connectionString)
        connection.Open()
        printfn "Connection successful"
        let query = "SELECT name, number, email FROM data_of_a7"
        use command = new MySqlCommand(query, connection)
        use reader = command.ExecuteReader()
        while reader.Read() do
            let name = reader.GetString(0)
            let number = reader.GetInt32(1)
            let email = reader.GetString(2)
            printfn "name: %s, number: %d, email: %s" name number email
    with
    | ex -> printfn "Error: %s" ex.Message

[<EntryPoint>]
let main argv =
    // إنشاء النافذة الرئيسية
    let form = new Form(Text = "F# CRUD App", Width = 600, Height = 400)

    // إنشاء الحقول
    let nameLabel = new Label(Text = "Name:", AutoSize = true, Top = 20, Left = 20)
    let nameTextBox = new TextBox(Width = 200, Top = 20, Left = 80)

    let numberLabel = new Label(Text = "Number:", AutoSize = true, Top = 60, Left = 20)
    let numberTextBox = new TextBox(Width = 200, Top = 60, Left = 80)

    let emailLabel = new Label(Text = "Email:", AutoSize = true, Top = 100, Left = 20)
    let emailTextBox = new TextBox(Width = 200, Top = 100, Left = 80)

    // زر "Search"
    let searchLabel = new Label(Text = "Search:", AutoSize = true, Top = 200, Left = 20)
    let searchTextBox = new TextBox(Width = 200, Top = 200, Left = 80)

    let searchButton = new Button(Text = "Search", Top = 200, Left = 300, Width = 100)
    searchButton.Click.Add(fun _ ->
        MessageBox.Show(sprintf "Searching for: %s" searchTextBox.Text)
        retrieveData()
    )

    // زر "Edit"
    let editButton = new Button(Text = "Edit", Top = 260, Left = 20, Width = 100)
    editButton.Click.Add(fun _ ->
        MessageBox.Show("Edit functionality not implemented yet.") |> ignore
    )

    // زر "Delete"
    let deleteButton = new Button(Text = "Delete", Top = 260, Left = 140, Width = 100)
    deleteButton.Click.Add(fun _ ->
        MessageBox.Show("Delete functionality not implemented yet.") |> ignore
    )

    // زر "Add"
    let addButton = new Button(Text = "Add", Top = 260, Left = 260, Width = 100)
    addButton.Click.Add(fun _ ->
        try
            let name = nameTextBox.Text
            let email = emailTextBox.Text
            let number = numberTextBox.Text

            if String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(number) then
                MessageBox.Show("Please fill all fields.") |> ignore
            else
                let connectionString = "Server=localhost;Database=tester;User Id=root;Password=;"
                use connection = new MySqlConnection(connectionString)
                connection.Open()

                let query = "INSERT INTO data_of_a7 (name, number,email) VALUES (@name, @number, @email)"
                use command = new MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@name", name) |> ignore
                command.Parameters.AddWithValue("@number", Int32.Parse(number)) |> ignore
                command.Parameters.AddWithValue("@email", email) |> ignore

                let rowsAffected = command.ExecuteNonQuery()
                if rowsAffected > 0 then
                    MessageBox.Show("Data added successfully!") |> ignore
                else
                    MessageBox.Show("Failed to add data.") |> ignore
        with
        | ex -> MessageBox.Show($"Error: {ex.Message}") |> ignore
    )

    // إضافة كل العناصر للنافذة
    form.Controls.AddRange(
        [| nameLabel; nameTextBox;
           numberLabel; numberTextBox;
           emailLabel; emailTextBox;

           searchLabel; searchTextBox; searchButton;
           editButton; deleteButton; addButton |]
    )

// Run the application
    Application.Run(form)
    0
