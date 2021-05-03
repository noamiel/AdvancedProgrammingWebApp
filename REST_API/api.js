const Joi = require('joi');
const express = require('express');
const app = express();
app.use(express.json());

const models = [
    { id: 1, name: "model1"},
    { id: 2, name: "model2"},
    { id: 3, name: "model3"},
]

const port = 9876;
app.listen(port, () => console.log(`Listening on port ${port}...`));

// main window
app.get('/', (req, res) => {
    res.send('Welcome');
});

app.post('/api/model', (req, res) => {
    // shape of object
    const schema = Joi.object({
        name: Joi.string().min(3).required()
    });
    const result = schema.validate(req.body);
    console.log(result);

    if (result.error) {
        res.status(400).send(result.error.details[0].message);
        return;
    }

    const model = {
        id: models.length + 1,
        name: req.body.name
    }
    models.push(model);
    // by convention
    res.send(model);
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
    res.json({ models: models })
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

