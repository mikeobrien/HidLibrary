exports.matchAll = function(regex, text) {
    var matches = [];
    var match = regex.exec(text);
    while (match !== null) {
        matches.push(match[1]);
        match = regex.exec(text);
    }
    return matches;
};