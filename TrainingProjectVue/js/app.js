const sitefinityNewsApi = 'http://localhost:7654/api/featured-articles/newsItems';
const sitefinityTokenURL = 'http://localhost:7654/Sitefinity/Authenticate/OpenID/connect/token';
const sitefinityIdentityServerClient = 'username=admin&password=admin@2&grant_type=password&scope=openid&client_id=testApp&client_secret=secret';

var app = new Vue({
    el: '#app',
    data: {
        Title: '',
        Summary: '',
        Content: '',
        Id:'',
        newsItems: []
    },

    mounted: function () {
        var page = window.location.pathname;
        this.getToken();

        if (page == "/index.html" || page == "/") {
            this.getNewsItems();
        }
        if (page == "/edit.html") {
            const params = new URLSearchParams(window.location.search);
            var newsId = params.get('id');
            console.log(newsId);
            this.getNewsItem(newsId);
        }
    },

    methods: {
        getToken: function () {

            var config = {
                method: 'post',
                url: sitefinityTokenURL,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Authorization': 'Bearer null'
                },
                data: sitefinityIdentityServerClient
            };

            axios(config)
                .then(function (response) {
                    sitefinityToken = response.data.access_token;
                })
                .catch(function (error) {
                    console.log(error);
                });
        },

        getNewsItems: function () {
            axios.get(sitefinityNewsApi)
                .then(function (response) {
                    app.newsItems = response.data.value;
                })
                .catch(function (error) {
                    console.log(error);
                });
        },

        getNewsItem: async function (id) {
            var url = sitefinityNewsApi + '/' + id;
            var response = await axios.get(url)
                .then(function (response) {
                
                    return response;
                })
                .catch(function (error) {
                    console.log(error);
                });

            this.Title = response.data.Title;
            this.Summary = response.data.Summary;
            this.Content = response.data.Content;
            this.Id = response.data.Id;

        },

        createNewsItem: function () {

            var data = JSON.stringify({ "Title": this.Title,"Summary":this.Summary,"Content":this.Content });

            var config = {
                method: 'post',
                url: sitefinityNewsApi,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sitefinityToken
                },
                data: data
            };

            axios(config)
                .then(function (response) {
                    console.log(JSON.stringify(response.data));
                    window.location.href = '/';
                })
                .catch(function (error) {
                    console.log(error);
                });

        },
        
        deleteNewsItem: function (id) {
            this.getToken();
            const deleteURL = sitefinityNewsApi + '(' + id + ')';
            const headers = { 'Authorization': 'Bearer ' + sitefinityToken}


            axios.delete(deleteURL, { headers })
                .then(function (response) {
                    console.log(response);
                    window.location.href = '/';
                })
                .catch(function (response) {
                    console.log(response)
                });

            
        },

        updateNewsItem: function () {
            var url = sitefinityNewsApi + '/' + this.Id;
            console.log(url);

            var data = JSON.stringify({ "Title": this.Title, "Summary": this.Summary, "Content": this.Content });

            var config = {
                method: 'patch',
                url: url,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sitefinityToken
                },
                data: data
            };

            axios(config)
                .then(function (response) {
                    console.log(JSON.stringify(response.data));
                    window.location.href = '/';
                })
                .catch(function (error) {
                    console.log(error);
                });           
        }

    }
})