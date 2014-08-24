// Taken from http://blog.strongloop.com/practical-examples-of-the-new-node-js-streams-api/

var stream = require('stream');
var lineStream = new stream.Transform({
    objectMode: true
});

lineStream._transform = function(chunk, encoding, done) {
    var data = chunk.toString();

    if (this._lastLineData) {
        data = this._lastLineData + data;
    }

    var lines = data.split('\n');

    this._lastLineData = lines.splice(lines.length - 1, 1)[0];

    lines.forEach(this.push.bind(this));

    done();
};

lineStream._flush = function(done) {

    if (this._lastLineData) {
        this.push(this._lastLineData);
    }

    this._lastLineData = null;

    done();

};

module.exports = lineStream;