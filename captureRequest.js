chrome.webRequest.onBeforeRequest.addListener(
    function (request) {
        console.log("Request URL:", request.url);
        console.log("Request Method:", request.method);
        if (request.requestBody) {
            let body = '';

            if (request.requestBody.raw) {
                const uint8Array = new Uint8Array(request.requestBody.raw[0].bytes);
                body = new TextDecoder("utf-8").decode(uint8Array);
            }

            console.log("Post Data:", body);
        }
    },
    { urls: ["<all_urls>"] },
    ["requestBody"]
);
  
chrome.webRequest.onCompleted.addListener(
    function (response) {
        console.log("\nResponse ---");
        console.log("URL:", response.url);
        console.log("Status Code:", response.statusCode);
        console.log("Method:", response.method);
        console.log("----------------\n");
    },
    { urls: ["*://*/*"] }
);