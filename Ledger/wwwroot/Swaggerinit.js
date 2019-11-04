function createSwaggerUi() {
    var full = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
    const ui = window.SwaggerUIBundle({
        url: full + "/swagger/v2/swagger.json",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
            window.SwaggerUIBundle.presets.apis,
            window.SwaggerUIStandalonePreset
        ],
        plugins: [
            window.SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: "StandaloneLayout",

        oauth2RedirectUrl: full + "oauth2-redirect.html"
    });
    window.ui = ui;
}
window.addEventListener("load", function () {
    createSwaggerUi();
});