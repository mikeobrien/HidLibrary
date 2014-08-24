var fs = require('fs');

var attributes = {
    title: "AssemblyTitle",
    description: "AssemblyDescription",
    configuration: "AssemblyConfiguration",
    company: "AssemblyCompany",
    product: "AssemblyProduct",
    copyright: "AssemblyCopyright",
    trademark: "AssemblyTrademark",
    culture: "AssemblyCulture",
    version: "AssemblyVersion",
    fileVersion: "AssemblyFileVersion"
};

exports.setAttributes = function(source, values) {
    var regex = function(attr) { return new RegExp('(\\[assembly\\: ' + attr + '\\(\\")(.*?)(\\"\\)\\])', 'g'); };
    var result = source;
    Object.keys(values).forEach(function(name) { 
        result = result.replace(regex(attributes[name]), '$1' + values[name] + '$3'); 
    });
    return result;
};

exports.setFileAttrbutes = function(files, values) {
    files.forEach(function(path) {
        fs.writeFileSync(path, exports.setAttributes(fs.readFileSync(path, 'utf8'), values));
    });
};