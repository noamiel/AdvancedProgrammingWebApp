/**
 * install first:
 * * all of the names inside require(name)
 * * nodemon (allows you to run the js file only once, and then when you change the code it restarts automatically)
 * * there may be modules which I missed. Install whatever the crash says you need
 */

// some imports and initializations
const express = require('express');
const fileUpload = require('express-fileupload');
const cors = require('cors');
const bodyParser = require('body-parser');
const morgan = require('morgan');
const _ = require('lodash');
const edge = require('edge-js');
const Joi = require('joi');

const app = express();

// enable files upload
app.use(fileUpload({
    createParentPath: true
}));

// add other middleware
app.use(cors());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(morgan('dev'));

const models_list = []

// initiate server
// in order to run it: nodemon api.js
const port = 9876;
app.listen(port, () => console.log(`Listening on port ${port}...`));

// get dlls and functions
// the dll is in `../temporary_files/simple_csharp_dll/ClassLibrary1`. This is a Visual Studio project.
// you can try to work with it.
const dll_path = '../temporary_files/simple_csharp_dll/ClassLibrary1/ClassLibrary1/bin/Debug/ClassLibrary1.dll';
let dll_name = require('path').join(__dirname, dll_path);

var learn_from_csv = edge.func({
    assemblyFile: dll_name,
    typeName: 'ClassLibrary1.Startup',  // change those names according to the relevant names 
    methodName: 'LearnCSV'              // This must be Func<object,Task<object>>
});

// main window
app.get('/', (req, res) => {
    res.send('Welcome')
})

// tarin new model. this is the request your'e looking for
app.post('/csv/model', async (req, res) => {

    // get model type from query line. send by /?model_type=<model_type>
    const schema_query = Joi.object({
        model_type: Joi.string().valid('regression', 'hybrid').required()
    });

    // validate query
    const result_query = schema_query.validate(req.query);

    // throw an error if wrong
    if (result_query.error) {
        res.status(400).send(result_query.error.details[0].message);
        return;
    }

    if (!req.files)
        return res.status(400).send("No file uploaded");
    if (!req.files.learn_csv)
        return res.status(400).send("`learn_csv` was not uploaded");
    
    // Use the name of the input field to retrieve the uploaded file
    let learn_csv = req.files.learn_csv;
    if (!learn_csv.name.endsWith(".csv"))
        return res.status(400).send("file must be of csv type")

    // Use the mv() method to place the file in upload directory (i.e. "uploads")
    learn_csv.mv('./uploads/' + learn_csv.name);

    // create model
    let id = models_list.length
    let model = new Model(id, (new Date()).toUTCString(), "pending")
    models_list.push(model)
    const uploaded_file_name = `${process.cwd()}/uploads/${learn_csv.name}`

    // run learn function based on type
    let data = {
        "csv_name": uploaded_file_name, 
        "model_type": req.query.model_type, 
        "passed_data_type": "csv"
    }

    // run c# code asynchoniously
    learn_from_csv(data, function (error, result) {
        if (error) throw error;
        models_list[id].status = "ready"
    })

    // push new model to list and send back by convention
    res.send(model)
});


// using this request you can see the uploaded models and see if the model had been processed yet
app.get('/api/models', (req, res) => {
    res.send(models_list);
});

// using this you can pick a specific model to show
app.get('/api/model', (req, res) => {
    // get model type from query line. send by /?model_type=<model_type>
    const schema_query = Joi.object({
        id: Joi.number().integer().required()
    });

    // validate query
    const result_query = schema_query.validate(req.query);
    console.log(result_query);

    // throw an error if wrong
    if (result_query.error) {
        res.status(400).send(result_query.error.details[0].message);
        return;
    }
    // first find the given model
    let model = models_list.find(c => c.id === parseInt(req.query.id));
    if (!model) return res.status(404).send("the model was not found");

    // return it if found
    res.send(model);
});



/* // train new model - REST api
// not actually needed but I keep it anyway in case some of what written in here would be useful/
app.post('/api/model', (req, res) => {
    // get model type from query line. send by /?model_type=<model_type>
    const schema_query = Joi.object({
        model_type: Joi.string().valid('regression', 'hybrid').required()
    });

    // validate query
    const result_query = schema_query.validate(req.query);
    console.log(result_query);

    // throw an error if wrong
    if (result_query.error) {
        res.status(400).send(result_query.error.details[0].message);
        return;
    }

    train_data = req.body.train_data;
    if (train_data === undefined)
        return res.status(400).send("train_data is required in request body");
    let data = new Data(train_data);

    let id = models_list.length
    let model = new Model(id, (new Date()).toUTCString(), "pending")
    models_list.push(model)

    // run learn function based on type
    learn_dictionary[req.query.model_type](data.dataJSON, function (error, result) {
        if (error) throw error;
        console.log(`the result is: ${result}`);
        models_list[id].status = "ready"
    })

    // push new model to list and send back by convention
    // models_list[id].status = "ready"
    res.send(model)
});

// the same as the previous one
app.get('/api/model/:id', (req, res) => {
    // first find the given model
    let model = models.find(c => c.id === parseInt(req.params.id));
    if (!model) return res.status(404).send("the model was not found");

    // return it if found
    res.send(model);
});

// again, not needed
app.delete('/api/model', (req, res) => {
    // first find the given model
    let model = models.find(c => c.id === parseInt(req.params.id));
    if (!model) return res.status(404).send("the model was not found");

    // delete it if found
    const index = models.indexOf(model);
    models.splice(index, 1);
    res.send(model);
}); */

// those are JavaScript classes which are needed for the project
// don't change `Model` but you can change the rest of them
class Model {
    constructor(id, upload_time) {
        this.id = id;
        this.upload_time = upload_time;
        this.status = "pending";
    }
    // some extra function may be needed
}

class Anomaly {
    constructor(anomalies, reason) {
        this.anomalies = anomalies;
        this.reason = reason;
    }
    // some extra function may be needed
}

class Span {
    constructor(start_time, end_time) {
        this.start_time = start_time;
        this.end_time = end_time;
    }
    // some extra function may be needed
}
