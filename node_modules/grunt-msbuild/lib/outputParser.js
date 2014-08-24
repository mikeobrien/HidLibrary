module.exports = function(line) {
    console.log('line...');
    if(line.indexOf('Done Building Project')){
        console.log(line);
    }
};