const Joi = require('joi');
const express = require('express');
const app = express();
app.use(express.json());
var edge = require('edge-js');

let dllName = require('path').join(__dirname, '../simple_csharp_dll/ClassLibrary1/ClassLibrary1/bin/Debug/ClassLibrary1.dll');
var add7 = edge.func({
    assemblyFile: dllName,
    typeName: 'Startup',
    methodName: 'Invoke' // This must be Func<object,Task<object>>
});

/**
 * you can also use a single file (for example, that uses a different dll)
 * the method should always be Invoke and the class should always be startup.
 * I ran it with this line:
 * var add7 = edge.func(require('path').join(__dirname, '../simple_csharp_project/ConsoleApp1/ConsoleApp1/Program.cs'));
 * for more info: https://github.com/agracio/edge-js
 */

app.get('/', (req, res) => {
    add7(10, function (error, result) {
        if (error) throw error;
        // console.log(result);
        res.send(`the result is: ${result}`);
    }); 
});

const port = 9877;
app.listen(port, () => console.log(`Listening on port ${port}...`));
