/**
 * Samenvatting van te kennen commando's
 * door Cédric D'hooge
 */


/*CONNECTIE
============*/

SqlConnection con = new SqlConnection("Data Source=SERVER; Initial Catalog=DATABASE; User ID=USERNAME; PASSWORD=PASSWORD");

/*OPENEN EN SLUITEN VERBINDING
==============================*/

con.Open();
con.Close();

/*COMMANDO
===========*/

SqlCommand cmd = new SqlCommand("QUERY", con);

/*DATA UITLEZEN
================*/

SqlDataReader rdr = cmd.ExecuteReader();

while (rdr.Read()) {
  Console.WriteLine(rdr[0]);
}

/*
ExecuteReader = QUERY uitvoeren en DataReader object terugkrijgen
ExecuteNonQuery = QUERY uitvoeren en beinvloede rijen terugkrijgen
ExecuteScalar = QUERY uitvoeren en de 1ste COLUMN van de 1ste ROW terugkrijgen
*/

/* DATA TABLES
===================*/

DataTable table = new DataTable();
table.Load(rdr, LoadOption.PreserveChanges)

/* DATA ADAPTER
===================*/

SqlDataAdapter daCustomers = new SqlDataAdapter("QUERY", con) // Sluit zelf de connectie af

/* COMMAND BUILDER
===================*/

SqlCommandBuilder cmdBldr = new SqlCommandBuilder(daCustomers) // Voegt automatisch een insert, update, deleter query toe aan de adapter

/* DATA SET
====================*/

DataSet dsCustomers = new DataSet();
daCustomers.Fill(dsCustomers, "TABLE")

/* DATA GRID
===================*/

dgCustomers.DataSource = dsCustomers;
dgCustomers.DataMember = "TABLE";

/* PARAMETERS SQL-INJECTIE
===========================*/

SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE city = @city", con);
SqlParameter par = new SqlParameter();
par.ParameterName = "@city";
par.Value = INPUT
cmd.Parameters.Add(par);

/* SPROC COMMAND
===================*/

SqlCommand cmd = new SqlCommand("SPROCNAAM", con);
cmd.commandType = CommandType.StoredProcedure;

/* SPROC BASICS
===================*/

DELIMITER $$
  DROP PROCEDURE IF EXISTS SPROCNAAM $$
  CREATE PROCEDURE SPROCNAAM()
  BEGIN
    SELECT "TEST";
  END $$
DELIMITER;
CALL SPROCNAAM();

/* SPROC IF-ELSE
===================*/

BLOCKNAME:BEGIN
  IF (VOORWAARDE) THEN
    LEAVE BLOCKNAME;
  END IF;

  IF (VOORWAARDE) THEN
    QUERY
  ELSE
    QUERY
  END IF;
END BLOCKNAME;

/* SPROC LOOPS
===================*/

// WHILE
BEGIN
  DECLARE v INT;
  SET v = 0;

  WHILE v < 5 DO
    INSERT INTO TABLE VALUES(v);
    SET v = v + 1;
  END WHILE;
END $$

// DO-WHILE
BEGIN
  DECLARE v INT;
  SET v = 0;

  REPEAT
    INSERT INTO TABLE VALUES(v);
    SET v = v + 1;
  UNTIL v >= 5 END REPEAT;
END $$

// LOOP WITH IF-STATEMENTS
BEGIN
  DECLARE v INT;
  SET v = 0;

  LOOP_LABEL: LOOP
    IF (VOORWAARDE) THEN
      SET v = v + 1;
      ITERATE LOOP_LABEL; // ITERATE = CONTINUE
    END IF;

    INSERT INTO TABLE VALUES(v);
    SET v = v + 1;

    IF (VOORWAARDE) THEN
      LEAVE LOOP_LABEL;
    END IF
  END LOOP;
END $$

/* SPROC ERROR HANDLING
========================*/

BEGIN
  DECLARE (EXIT/CONTINUE) HANDLER FOR SQLSTATE (NUMBER/ERRORSTRING/CONDITION) SET @x = 2;
END $$

/* SPROC CURSORS
===================*/

// CURSOR loopt resultaat per resultaat af
BEGIN
  DECLARE CURSORNAME CURSOR FOR SELECT TABLE;
  OPEN CURSORNAME;
    FETCH CURSORNAME INTO VARIABLE;
  CLOSE CURSORNAME;
END $$

/* TRANSACTIONS ACID
=====================*/

A (atomicity) = Ofwel geheel uitgevoerd ofwel niet
C (consistency) = Creëert nieuwe geldige staat of herstelt de oude
I (isolation) = Transacties hebben geen zicht in elkaars tussenresultaten
D (durability) = Voltooide transacties kunnen niet ongedaan gemaakt worden

/* TRANSACTIONS CODE
=====================*/

using (SqlConnection con) {
  con.open();
  SqlTransaction trans = con.BeginTransaction();
  (trans.Commit()/trans.Rollback();) => Commit or undo the transaction
}

SqlCommand updateCmd = new SqmCommand(QUERY, con, trans);
UpdateCmd.Transaction = trans;

/* PROVIDER FACTORY
====================*/

machine.config // file met providers
DataTable table = DbProviderFactories.GetFactoryClasses(); // Retrieve installed providers and factories

/* FACTORY CODE
===================*/

DbProviderFactory factory = DbProviderFactories.GetFactory(PROVIDERNAME);
DbConnection con = factory.CreateConnection();
con.ConnectionString = CONNECTIONSTRING;

/*
LINQ-Volgorde
==============
*/

from string c in colors where c.StartsWith("B") orderby c select c

1) from
2) where
3) orderby
4) select

/*
Wat maakt LINQ LINQ?
=====================

- Object Initializers = */ ( Car c = new Car() { VIN = "ABC123", Make= "Ford", Model = "F-250", Year = 2000 }; ) /*

  Object Initializers in LINQ (PROJECTIE)
  ========================================

  - Transformatie van data in een LINQ query om enkel die zaken te krijgen die je nodig hebt.

  Voorbeeld.
  ==========

  Je wilt enkel die kleuren krijgen die woordlengte hebben van 5 karakters gesorteerd op kleur */
  string[] colors = { "Red", "Brown", "Orange" };
  IEnumerable<Car> fords = from c in colors where c.Length == 5 orderby c select new Car() { Make = "Ford", Color = c };

  /*
  Collection Initializers in LINQ
  ================================

  - Zelfde als object initializers maar dan voor collections

  Voorbeeld.
  ===========
*/

  private List<Car> GetCars() {
    return new List<Car> {
      new Car { VIN = "ABC123", Make = "Ford", Model = "F-250", Year = 2000 },
      new Car { VIN = "DEF123", Make = "BMW", Model = "Z-3", Year = 2005 }
    }
  }

/*
- Implicit Typed Local Variable Declarations = */( Car c = new Car(); )/*

- Anonieme Types = */( var x = new { Make = "VW", Model = "Bug" }; )/*

  Anonieme Types in LINQ
  =======================

  - Anonieme types worden gebruikt voor projecties.

  Voorbeeld.
  ==========
*/

  - var carData = from c in GetCars()
          where c.Year >= 2000
          orderby c.Year
  BEGIN =====>  select new {
            c.VIN,
            MakeAndModel = c.Make + " " + c.Model
          };  <=====  END

/*
- Lambda expressies = */( var found = cars.Find(c => c.Year == 2000) )/* ===> (c => c.Year == 2000) is de lambda expressie

- Extension methods = extension methodes zijn statische methodes die je toelaten om een methode toe te voegen aan een type

- Query extension methods = GetCars().All(c => c.Year > 1960) || Any() || Average() || Cast() || Concat()
*/

/* DB CONTEXT
===================*/

public class CONTEXTNAME : DbContext {
  public DbSet<Blog> Blogs {get; set;} // Blog = CLASS
  public DbSet<Post> Posts {get; set;} // Post = CLASS
}

/* INHERITENCE EF6
===================*/

public class CLASS {
  public virtual List<CLASS> Posts {get; set;} // Overerving met virtual
}

/* RETRIEVE DATA EF6
=====================*/

using (var db = new CONTEXTNAME()) {
  var blog = new Blog { Name = "test"};
  db.Blogs.Add(blog); // Add blog to database
  db.SaveChanges();

  var query = from b in db.blogs orderby b.Name select b; // Retrieve data from database
}

/*
CAP DRIEHOEK
=============*/

C = Consistency => All clients always have the same view of the data
A = Availability => Each client can always read and write
P = Partition Tolerance => The system works well despite physical network partitions

/*
NoSQL systemen geplaatst in de DRIEHOEK
========================================*/

CA => Vertica en Greenplum => Alle clienten hebben altijd hetzelfde overzicht van de data en ze kunnen altijd lezen en schrijven
AP => Cassandra en SimpleDB => Elke client kan lezen en schrijven en het systeem werkt goed zonder fysische netwerk partities
CP => MongoDB en Hbase => Alle clienten hebben altijd hetzelfde overzicht van de data en het systeem werkt goed zonder fysische netwerk partities

/* MONGO DB
===================*/

MongoClient client = new MongoClient("mongodb://localhost");
var db = client.GetDatabase("test");
IMongoCollection<Person> personen = db.GetCollection<Person>("personen"); // Retrieve personen table from database
List<Person> test = personen.Find(x => x.Age > 40).ToListAsync(); // Retrieve persons from collections
