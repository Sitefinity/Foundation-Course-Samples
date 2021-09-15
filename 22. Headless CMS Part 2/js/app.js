const sitefinityNewsApi =
    'https://localhost:44331/api/featured-articles/newsitems';

const sitefinityCreateNewsApi =
    'https://localhost:44331/api/default/newsitems';

const sitefinityAuthorApi =
    'https://localhost:44331/api/default/Default.GetUsers()';

const sitefinityTokenURL =
  'https://localhost:44331/sitefinity/oauth/token';

const sitefinityAuthCreds = 'username=service@test.test&password=admin@2&grant_type=password&client_id=testApp&client_secret=secret';

var app = new Vue({
  el: '#app',
  data: {
    Title: '',
    Summary: '',
    Content: '',
    Id: '',
    Author: '',
    authors: [],
    newsItems: [],
    orderByDirection: 'desc',
    search: false,
    searchTerm: '',
  },
  mounted: function () {
    var page = window.location.pathname;
    this.getToken();
    this.getAuthors();

    if (page == '/index.html' || page == '/') {
      this.getNewsItems(sitefinityNewsApi);
    }
    if (page == '/edit.html') {
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
          'Content-Type': 'application/x-www-form-urlencoded'
        },
          data: sitefinityAuthCreds,
      };

      axios(config)
        .then(function (response) {
          sitefinityToken = response.data.access_token;
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    getNewsItems: function (url) {
      // * Resetting to indicate loading. Remove the next line if the flash is driving you crazy
      this.newsItems = [];
      axios
        .get(url)
        .then(function (response) {
          app.newsItems = response.data.value;
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    getNewsItem: function (id) {
      var app = this;
      var url = sitefinityNewsApi + '/' + id;
      axios
        .get(url)
        .then(function (response) {
          app.Title = response.data.Title;
          app.Summary = response.data.Summary;
          app.Content = response.data.Content;
          app.Id = response.data.Id;
          app.Author = response.data.Author;
          return response;
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    getAuthors: function () {
      var app = this;
      axios
        .get(sitefinityAuthorApi) 
        .then(function (response) {
            var jsonData = JSON.parse(response.data.value[0].Data);
            app.authors = jsonData.map((data) => ({
                Name: data.Name
            }));
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    createNewsItem: function () {
      var data = JSON.stringify({
        Author: this.Author,
        Title: this.Title,
        Summary: this.Summary,
        Content: this.Content,
        PublicationDate: new Date().toISOString(),
      });

      var config = {
        method: 'post',
        url: sitefinityCreateNewsApi,
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + sitefinityToken,
        },
        data: data,
      };

      axios(config)
        .then(function (response) {
          console.log(JSON.stringify(response.data));
        })
        .catch(function (error) {
          console.log(error);
        });
    },

    deleteNewsItem: function (id) {
      this.getToken();
      const deleteURL = sitefinityNewsApi + '(' + id + ')';
      const headers = { Authorization: 'Bearer ' + sitefinityToken };

      axios
        .delete(deleteURL, { headers })
        .then(function (response) {
          console.log(response);
        })
        .catch(function (response) {
          console.log(response);
        });
    },

    updateNewsItem: function () {
      var url = sitefinityNewsApi + '/' + this.Id;
      console.log(url);

      var data = JSON.stringify({
        Title: this.Title,
        Summary: this.Summary,
        Content: this.Content,
        Author: this.Author,
      });

      var config = {
        method: 'patch',
        url: url,
        headers: {
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + sitefinityToken,
        },
        data: data,
      };

      axios(config)
        .then(function (response) {
          console.log(JSON.stringify(response.data));
        })
        .catch(function (error) {
          console.log(error);
        });
    },
    toggleOrderByDirection: function () {
      if (this.orderByDirection === 'desc') {
        this.orderByDirection = 'asc';
      } else {
        this.orderByDirection = 'desc';
      }
    },
    submitSearch: function () {
      // * Keep track if the user has searched for something or not
      if (this.searchTerm !== '') {
        this.search = true;
      } else {
        this.search = false;
      }

      // * Add the url parameter for filtering
      var url =
        sitefinityNewsApi +
        "?$filter=contains(Title,'" +
        this.searchTerm +
        "')";

      this.getNewsItems(url);
    },
  },

  watch: {
    orderByDirection: function (newVal) {
      // * Add the url parameter for ordering by the Publication Data
      var url = sitefinityNewsApi + '?$orderby=PublicationDate ' + newVal;
      // * Add the search filter to the url if a user has an active search term
      if (this.search) {
        url = url + "&$filter=contains(Title,'" + this.searchTerm + "')";
      }

      this.getNewsItems(url);
    },
  },
});
