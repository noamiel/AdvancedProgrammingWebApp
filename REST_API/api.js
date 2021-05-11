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

//add other middleware
app.use(cors());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(morgan('dev'));

// main window
app.get('/', (req, res) => {
    res.send('Welcome')
})

const models_list = []

const port = 9876;
app.listen(port, () => console.log(`Listening on port ${port}...`));

// get dlls and functions. May have to change that part
const dll_path = '../temporary_files/simple_csharp_dll/ClassLibrary1/ClassLibrary1/bin/Debug/ClassLibrary1.dll';
let dll_name = require('path').join(__dirname, dll_path);
var learn_regression = edge.func({
    assemblyFile: dll_name,
    typeName: 'ClassLibrary1.Startup', // change those names according to the relevant names 
    methodName: 'Regression' // This must be Func<object,Task<object>>
});

var learn_hybrid = edge.func({
    assemblyFile: dll_name,
    // nameSpace: 'ClassLibrary1',
    typeName: 'ClassLibrary1.Startup', // change those names according to the relevant names 
    methodName: 'Hybrid' // This must be Func<object,Task<object>>
});

var learn_hybrid_csv = edge.func({
    assemblyFile: dll_name,
    // nameSpace: 'ClassLibrary1',
    typeName: 'ClassLibrary1.Startup', // change those names according to the relevant names 
    methodName: 'HybridCSV' // This must be Func<object,Task<object>>
});

const learn_dictionary = {
    'hybrid': learn_hybrid,
    'regression': learn_regression
}

app.post('/upload-avatar', async (req, res) => {

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

    if (!req.files)
        return res.status(400).send("No file uploaded");
    // Use the name of the input field (i.e. "avatar") to retrieve the uploaded file
    let avatar = req.files.avatar;
    if (!avatar.name.endsWith(".csv"))
        return res.status(400).send("file must be of csv type")

    // Use the mv() method to place the file in upload directory (i.e. "uploads")
    avatar.mv('./uploads/' + avatar.name);

    let id = models_list.length
    let model = new Model(id, (new Date()).toUTCString(), "pending")
    models_list.push(model)
    const uploaded_file_name = `${process.cwd()}/uploads/${avatar.name}`
    // console.log(`${process.cwd()}/uploads/${avatar.name}`)
    // var loc = window.location.pathname;

    // run learn function based on type
    let data = {
        "csv_name": uploaded_file_name, 
        "model_type": req.query.model_type, 
        "passed_data_type": "csv"
    }
    learn_hybrid_csv(data, function (error, result) {
        if (error) throw error;
        console.log(`the result is: ${result}`);
        models_list[id].status = "ready"
    })

    // push new model to list and send back by convention
    res.send(model)
    
    /* // send response
    res.send({
        status: true,
        message: 'File is uploaded',
        data: {
            name: avatar.name,
            mimetype: avatar.mimetype,
            size: avatar.size
        }
    }); */
});

// train new model
app.post('/csv/model', (req, res) => {
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
    res.send(model)
});

// train new model
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

// train new model
app.post('/api/anomaly', (req, res) => {
    // get model type from query line. send by /?model_type=<model_type>
    const schema_query = Joi.object({
        model_id: Joi.number().integer().min(0).max(models_list.length).required()
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


app.get('/api/models', (req, res) => {
    res.send(models_list);
});

app.get('/api/model', (req, res) => {
    // first find the given model
    let model = models.find(c => c.id === parseInt(req.query.id));
    if (!model) return res.status(404).send("the model was not found");

    // return it if found
    res.send(model);
});

// TODO: write a second request for each confusing one, such as here
app.get('/api/model/:id', (req, res) => {
    // first find the given model
    let model = models.find(c => c.id === parseInt(req.params.id));
    if (!model) return res.status(404).send("the model was not found");

    // return it if found
    res.send(model);
});

app.delete('/api/model', (req, res) => {
    // first find the given model
    let model = models.find(c => c.id === parseInt(req.params.id));
    if (!model) return res.status(404).send("the model was not found");

    // delete it if found
    const index = models.indexOf(model);
    models.splice(index, 1);
    res.send(model);
});

app.get('/api/models', (req, res) => {
    // res.send(models);
    res.json({
        models: models
    })
});

/* app.post('/api/model', (req, res) => {
    // its body would be copied (or very similar to) model post
    // TODO: use redirection somehow. May be useful also some places else
}); */

/*** OBJECTS DEFINITIONS ***/
// By convention, constructor function
// start with Uppercase letter.
/**
 * TODO:
 * 1) write also data
 * 2) throw errors
 */

class Model {

    constructor(id, upload_time) {
        this.id = id;
        this.upload_time = upload_time;
        this.status = "pending";
    }
    // some extra function may be needed
}

class Data {
    constructor(dataJSON) {
        this.dataJSON = dataJSON;
        this.names = Object.keys(dataJSON);
        console.log(this.names);
    }
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
// When used with new, assigns this = {}
// and returns this at the end.
// {“name”: “Jack”, “isAdmin”: false}
// example: let Model = new Model("Jack");
