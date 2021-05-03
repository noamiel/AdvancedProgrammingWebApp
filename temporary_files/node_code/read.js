const Joi = require('joi');
const express = require('express');
const app = express();
app.use(express.json());
var edge = require('edge-js');

// var myFunction = edge.func(getx);
// var add7 = edge.func(require('path').join(__dirname, '../simple_csharp_project/ConsoleApp1/ConsoleApp1/Program.cs'));
// C:\Users\buein\source\repos\AdvancedProgrammingWebApp\temporary_files\simple_csharp_dll\ClassLibrary1\ClassLibrary1\bin\Debug
// var add7 = edge.func(require('path').join(__dirname, '../simple_csharp_dll/ClassLibrary1/ClassLibrary1/bin/Debug/ClassLibrary1.dll'));
let dllName = require('path').join(__dirname, '../simple_csharp_dll/ClassLibrary1/ClassLibrary1/bin/Debug/ClassLibrary1.dll');
var add7 = edge.func({
    assemblyFile: dllName,
    typeName: 'Startup',
    methodName: 'Invoke' // This must be Func<object,Task<object>>
});

app.get('/', (req, res) => {
    add7(10, function (error, result) {
        if (error) throw error;
        // console.log(result);
        // console.log("message");
        res.send(`the result is: ${result}`);
        // res.send(result);
    }); 
    
});

// app.get('/', (req, res) => {
//     res.send('Hello!');
// });

const port = 9877;
app.listen(port, () => console.log(`Listening on port ${port}...`));
