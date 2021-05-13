window.Clipboard = {
    copyTo: function(text) {
        navigator.clipboard.writeText(text)
            .then(function () {
                console.log(text);
                alert("Short URL copied to clipboard!");
            })
            .catch(function (error) {
                alert(error);
            });
    }
};