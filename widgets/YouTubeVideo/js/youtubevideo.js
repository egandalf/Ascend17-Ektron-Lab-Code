Ektron.ready(function() {
    if(typeof (Ektron.Widget) == "undefined") {
        Ektron.Widget = {};
    }

    if(typeof (Ektron.Widget.YouTubeVideo) == "undefined") {

        Ektron.Widget.YouTubeVideo =
        {
            //Before getting started, please read this article that covers how to generate your v3 youtube api key
            //https://developers.google.com/youtube/v3/getting-started

            // PROPERTIES
            lastPage: 0,
            maxCount: 0,
            pageSize: 5, //max 50
            pageNumber: 0,
            sortBy: "relevance", // not currently working. options include date, rating, relevance, title, videoCount, viewCount
            sortOrder: "DESC",  // Not used in V3 options include ascending (ASC) or descending (DESC).            
            widgets: [],
            currentCallWidgetID: "",
            youtubeAPIKey: '', // Please see this link on how to generate your api key https://www.youtube.com/watch?v=Im69kzhpR3I
            nextPage: '',
            prevPage: '',
            searchUp: true,
            safeSearch: 'strict', //values can be none, moderate, strict


            GetStartIndex: function() {
                // summary:
                //      Return start index of video

                return Ektron.Widget.YouTubeVideo.pageNumber <= 0
                        ? 1
                        : (Ektron.Widget.YouTubeVideo.pageNumber * Ektron.Widget.YouTubeVideo.pageSize) + 1;
            },

            // CLASS OBJECTS
            YouTubeVideoWidget: function(id, outputId, submitButtonId) {
                var obj = this;
                obj.id = id;
                obj.submitBtn = $ektron("#" + submitButtonId);
                obj.output = $ektron("#" + outputId);

                // The hidden input control that save the value of youtube api key 
                obj.youtubeAPIKeyControl = $ektron("#" + id + 'youtubeAPIKey')[0];

                // Get youtube api key in this wedget globel settings
                Ektron.Widget.YouTubeVideo.youtubeAPIKey = obj.youtubeAPIKeyControl.value;

                var token = "aa";
                if(token.length > 1) {

                    obj.VideoClicked = function(id) {
                        // summary:
                        //      Submit video is when user select a video in list
                        // id: [string]
                        //      The id of youtube video

                        obj.output.attr("value", id);
                        obj.submitBtn.click();
                    };


                    obj.FindVideos = function() {
                        // summary:
                        //      Get the youtube videos depend on sort order and page number selected

                        // Do nothing if there are not youtube api key
                        // that key should be updated in youtube widget global settings
                        if(Ektron.Widget.YouTubeVideo.youtubeAPIKey == '') {
                            alert('Please set your youtube API key in the Youtube Widget global settings. Visit this link on how to generate your api key https://www.youtube.com/watch?v=Im69kzhpR3I');
                            $('.ytcancelbutton').click();
                        }
                        else {
                            // Youtube video sort order by
                            var oSort_by = $ektron("#" + id + "sort_by")[0];
                            Ektron.Widget.YouTubeVideo.sortBy = oSort_by.options[oSort_by.selectedIndex].value;

                            // Collect query parameters
                            var queryUrl = obj.GetDataUrl(Ektron.Widget.YouTubeVideo.sortBy);
                            queryUrl += "?max-results=" + Ektron.Widget.YouTubeVideo.pageSize + "&start-index=" + Ektron.Widget.YouTubeVideo.GetStartIndex();

                            // If sort by contain recent then get in time
                            if(Ektron.Widget.YouTubeVideo.sortBy.indexOf('recent') == -1) {
                                queryUrl += "&time=today";
                            }

                            Ektron.Widget.YouTubeVideo.currentCallWidgetID = obj.id;
                            obj.AppendScriptTag(queryUrl, 'searchYVideo' + obj.id, 'callBackDisplayVideos');
                        }

                    };

                    obj.DecreasePageNumber = function() {
                        // summary:
                        //      Decreasing one unit for page number

                        if(Ektron.Widget.YouTubeVideo.pageNumber <= 0) {
                            Ektron.Widget.YouTubeVideo.pageNumber = 0;
                        }
                        else {
                            Ektron.Widget.YouTubeVideo.pageNumber -= 1;
                        }
                    };

                    obj.IncreasePageNumber = function() {
                        // summary:
                        //      Increasing one unit for page number

                        if(Ektron.Widget.YouTubeVideo.pageNumber >= Ektron.Widget.YouTubeVideo.lastPage) {
                            Ektron.Widget.YouTubeVideo.pageNumber = Ektron.Widget.YouTubeVideo.lastPage;
                        }
                        else {
                            Ektron.Widget.YouTubeVideo.pageNumber += 1;
                        }
                    };

                    obj.PreviousVideos = function() {
                        // summary:
                        //      Display previous page

                        obj.DecreasePageNumber();
                        obj.FindVideos();
                    };

                    obj.NextVideos = function() {
                        // summary:
                        //      Display next page

                        obj.IncreasePageNumber();
                        obj.FindVideos();
                    };

                    obj.FirstVideos = function() {
                        // summary:
                        //      Display first page

                        Ektron.Widget.YouTubeVideo.ResetPages();
                        Ektron.Widget.YouTubeVideo.pageNumber = 0;
                        obj.FindVideos();
                    };

                    obj.LastVideos = function() {
                        // summary:
                        //      Display last page

                        Ektron.Widget.YouTubeVideo.pageNumber = Ektron.Widget.YouTubeVideo.lastPage;
                        obj.FindVideos();
                    };

                    obj.SearchPreviousVideos = function() {
                        // summary:
                        //      Search videos for previous page

                        $('.SearchLink').animate({ scrollTop: 0 }, 'fast');
                        Ektron.Widget.YouTubeVideo.nextPage = '';
                        Ektron.Widget.YouTubeVideo.searchUp = false;

                        obj.DecreasePageNumber();
                        obj.SearchVideos();

                    };

                    obj.SearchNextVideos = function() {
                        $('.SearchLink').animate({ scrollTop: 0 }, 'fast');
                        Ektron.Widget.YouTubeVideo.searchUp = true;
                        Ektron.Widget.YouTubeVideo.prevPage = '';

                        obj.IncreasePageNumber();
                        obj.SearchVideos();

                    };

                    obj.SearchFirstVideos = function() {
                        Ektron.Widget.YouTubeVideo.searchUp = false;
                        Ektron.Widget.YouTubeVideo.prevPage = '';
                        Ektron.Widget.YouTubeVideo.nextPage = '';
                        Ektron.Widget.YouTubeVideo.ResetPages();
                        Ektron.Widget.YouTubeVideo.pageNumber = 0;
                        obj.SearchVideos();
                    };

                    obj.SearchLastVideos = function() {
                        Ektron.Widget.YouTubeVideo.pageNumber = Ektron.Widget.YouTubeVideo.lastPage;
                        obj.SearchVideos();
                    };

                    obj.SearchVideos = function() {
                        // summary:
                        //      

                        var tbData = $ektron("#" + id + "SearchText");
                        var searchtext = tbData.val();
                        if(searchtext.length <= 0) {
                            return;
                        }

                        // Result vidoes was sort order by
                        var oSort_by = $ektron("#" + id + "sort_by1")[0];
                        Ektron.Widget.YouTubeVideo.sortBy = oSort_by.options[oSort_by.selectedIndex].value;

                        // Init the query parameters
                        var queryUrl = obj.GetDataUrl("");

                        queryUrl += "?part=snippet&type=video&maxResults=" + Ektron.Widget.YouTubeVideo.pageSize;
                        queryUrl += "&order=" + Ektron.Widget.YouTubeVideo.sortBy;
                        queryUrl += "&safeSearch=" + Ektron.Widget.YouTubeVideo.safeSearch;

                        if(Ektron.Widget.YouTubeVideo.searchUp) {
                            if(Ektron.Widget.YouTubeVideo.nextPage != '') {
                                queryUrl += '&pageToken=' + Ektron.Widget.YouTubeVideo.nextPage
                            }
                        }
                        else {
                            if(Ektron.Widget.YouTubeVideo.prevPage != '') {
                                queryUrl += '&pageToken=' + Ektron.Widget.YouTubeVideo.prevPage
                            }
                        }

                        var searchtype = $ektron("#" + id + "searchtype").val();
                        if(searchtype == "TAG") {
                            queryUrl += '&category=' + searchtext;
                        } else {
                            queryUrl += "&q=" + searchtext;
                        }

                        queryUrl += "&key=" + Ektron.Widget.YouTubeVideo.youtubeAPIKey;

                        $.ajax({
                            url: queryUrl,
                            async: false,
                            success: function(data) {
                                obj.DisplaySearchVideos(data);
                            },
                            error: function(data1, data2) {
                                alert(data1.responseText);
                            }
                        });

                        Ektron.Widget.YouTubeVideo.currentCallWidgetID = obj.id;
                        //obj.AppendScriptTag(queryUrl,'searchYVideo' + obj.id ,'callBackDisplaySearchVideos');
                    };

                    obj.MakeVideoThumbnail = function(thumbnailURL) {
                        // summary:
                        //      Create image as thumbnail style
                        // thumbnailURL: [string]
                        //      The video thumbnail image url

                        var thumbnail = $ektron("<img></img>");
                        thumbnail.attr("src", thumbnailURL);
                        thumbnail.attr("class", "thumbnail");
                        return thumbnail;
                    };

                    obj.MakeVideoLink = function(id, name) {
                        // summary:
                        //      Create link for youtube video
                        // id: [string]
                        //      The video id
                        // name: [string]
                        //      The video title

                        var videoLink = $ektron("<a></a>");
                        videoLink.attr("href", "");
                        videoLink.html(name);
                        return videoLink;
                    };

                    obj.MakeVideoShortDescription = function(shortDescription) {
                        // summary:
                        //      Create short description for youtube video
                        // shortDescription: [string]
                        //      The video description

                        var description = $ektron("<span></span>");
                        description.attr("class", "short-description");
                        description.html(shortDescription);
                        return description;
                    };

                    obj.DisplayVideos = function(videoCollection) {
                        // summary:
                        //      Display videos collection in widget
                        // videoCollection: [object]
                        //

                        // Reset videos list
                        var list = $ektron("#" + obj.id + " ul.video-list");
                        list.html("");

                        // Do nothing if there are not any videos
                        if(!videoCollection.feed) {
                            Ektron.Widget.YouTubeVideo.Pagingbuttons(obj.id, 0, 0);
                            return;
                        }

                        // Do nothing if there are not any videos
                        Ektron.Widget.YouTubeVideo.maxCount = videoCollection.feed.openSearch$totalResults.$t;
                        if(Ektron.Widget.YouTubeVideo.maxCount <= 0) {
                            Ektron.Widget.YouTubeVideo.Pagingbuttons(obj.id, 0, 0);
                            return;
                        }

                        var alt = false;
                        var itemIndex = -1;

                        // Loops through entries in the feed and calls appendVideoData for each
                        //for (var i in videoCollection.items) 
                        for(var i = 0, entry; entry = videoCollection.feed.entry[i]; i++) {

                            // Increase index
                            itemIndex++;

                            // Get video id
                            var videoID = entry.id.$t.substring(entry.id.$t.lastIndexOf('/') + 1);

                            var listItem = $ektron("<li></li>");
                            if(i === 0) {
                                // This is first item
                                listItem.attr("class", "alt1 videoFirst clearfix");
                            }
                            else {
                                listItem.attr("class", (alt = !alt) ? "alt1 clearfix" : "alt2 clearfix");
                            }

                            var thumbnail = obj.MakeVideoThumbnail(entry.media$group.media$thumbnail[0].url);
                            var videoLink = obj.MakeVideoLink('#', entry.media$group.media$title.$t);
                            var description = obj.MakeVideoShortDescription(entry.media$group.media$description.$t);

                            listItem.append("<div class='videoThumbOuter'></div>");
                            listItem.find(".videoThumbOuter").append("<div class='videoThumbInner'></div>");
                            listItem.find(".videoThumbInner").append(thumbnail.get(0));

                            listItem.append(videoLink.get(0));
                            listItem.append(description.get(0));

                            listItem.hover(function() {
                                $ektron(this).addClass('hover');
                            },
                                function() {
                                    $ektron(this).removeClass('hover');
                                }
                            );
                            listItem.click(
                                (function(id) {
                                    return function() {
                                        obj.VideoClicked(id);
                                        return false;
                                    };
                                }
                                )(videoID)
                            );
                            list.append(listItem);
                        }

                        // Update paging control
                        Ektron.Widget.YouTubeVideo.Pagingbuttons(obj.id, Ektron.Widget.YouTubeVideo.maxCount, itemIndex);
                    };

                    obj.DisplaySearchVideos = function(videoCollection) {
                        // summary:
                        //      Display videos collection in widget
                        // videoCollection: [object]
                        //      

                        // Reset videos list
                        var list = $ektron("#" + obj.id + " ul.video-search");
                        list.html("");

                        // Update paging control
                        Ektron.Widget.YouTubeVideo.maxCount = videoCollection.pageInfo.totalResults;
                        if(Ektron.Widget.YouTubeVideo.maxCount <= 0) {
                            Ektron.Widget.YouTubeVideo.PagingbuttonsSearch(obj.id, 0, 0);
                            return;
                        }
                        else {
                            Ektron.Widget.YouTubeVideo.PagingbuttonsSearch(obj.id, Ektron.Widget.YouTubeVideo.maxCount, videoCollection.items.length - 1);
                        }

                        // Update next page
                        try {
                            Ektron.Widget.YouTubeVideo.nextPage = videoCollection.nextPageToken;
                        }
                        catch(Exception) {
                            Ektron.Widget.YouTubeVideo.nextPage = '';
                        }

                        // Update previous page
                        try {
                            Ektron.Widget.YouTubeVideo.prevPage = videoCollection.prevPageToken;
                        }
                        catch(Exception) {
                            Ektron.Widget.YouTubeVideo.prevPage = '';
                        }

                        var alt = false;
                        var itemIndex = -1;
                        for(var i = 0; i < videoCollection.items.length; i++) {

                            // Increase index
                            itemIndex++;

                            var videoID = videoCollection.items[i].id.videoId;

                            var listItem = $ektron("<li></li>");
                            if(itemIndex == 0) {
                                // This is first item
                                listItem.attr("class", "alt1 videoFirst clearfix");
                            }
                            else {
                                listItem.attr("class", (alt = !alt) ? "alt1 clearfix" : "alt2 clearfix");
                            }

                            var thumbnail = obj.MakeVideoThumbnail(videoCollection.items[i].snippet.thumbnails.default.url);
                            var videoLink = obj.MakeVideoLink('#', videoCollection.items[i].snippet.title);
                            var description = obj.MakeVideoShortDescription(videoCollection.items[i].snippet.description);

                            listItem.append("<div class='videoThumbOuter'></div>");
                            listItem.find(".videoThumbOuter").append("<div class='videoThumbInner'></div>");
                            listItem.find(".videoThumbInner").append(thumbnail.get(0));

                            listItem.append(videoLink.get(0));
                            listItem.append(description.get(0));

                            listItem.hover(function() { $ektron(this).addClass('hover'); },
                                           function() { $ektron(this).removeClass('hover'); });
                            listItem.click((function(id) {
                                return function() {
                                    obj.VideoClicked(id);
                                    return false;
                                };
                            })(videoID));
                            list.append(listItem);
                        }

                        Ektron.Widget.YouTubeVideo.PagingbuttonsSearch(obj.id, Ektron.Widget.YouTubeVideo.maxCount, itemIndex);
                    };

                    obj.PlayerItem = function(title, id) {
                        // summary:
                        //      
                        // title: [string]
                        //      The video title
                        // id: [string]
                        //      The video id

                        var item = $ektron("<li></li>");
                        item.html(title);
                        item.click(function() {
                            obj.PlayerClicked(title, id);
                        });
                        return item.get(0);
                    };

                    obj.KeyPressHandler = function(elem, event, id) {
                        // summary:
                        //      Handle enter key press
                        // elem: [DOM]
                        //      The dom element
                        // event: [object]
                        //      The event of dom element
                        // id: [string]
                        //      The video id

                        // Execute search videos if user press enter key
                        if(event.keyCode == 13) {
                            if(event.preventDefault) event.preventDefault();
                            if(event.stopPropagation) event.stopPropagation();
                            event.returnValue = false;
                            event.cancel = true;
                            Ektron.Widget.YouTubeVideo.ResetPages();
                            setTimeout('Ektron.Widget.YouTubeVideo.widgets["' + id + '"].SearchVideos()', 1);
                            return false;
                        }
                    };

                    obj.GetDataUrl = function() {
                        // summary:
                        //      Return search url of google api service of youtube

                        return 'https://www.googleapis.com/youtube/v3/search/';
                    };

                    obj.AppendScriptTag = function(scriptSrc, scriptId, scriptCallback) {
                        // summary:
                        //      Append script inside HEAD tag element
                        // scriptSrc: [string]
                        //      The query url
                        // scriptId: [string]
                        //      The script id
                        // scriptCallback: [string]
                        //      The callback function

                        // Remove any old existance of a script tag by the same name
                        var oldScriptTag = document.getElementById(scriptId);
                        if(oldScriptTag) {
                            oldScriptTag.parentNode.removeChild(oldScriptTag);
                        }

                        // Create new script tag
                        var script = document.createElement('script');
                        script.setAttribute('src', scriptSrc + '&alt=json-in-script&callback=' + scriptCallback);
                        script.setAttribute('id', scriptId);
                        script.setAttribute('type', 'text/javascript');

                        // Append the script tag to the head to retrieve a JSON feed of videos
                        // NOTE: This requires that a head tag already exists in the DOM at the
                        // time this function is executed.
                        document.getElementsByTagName('head')[0].appendChild(script);
                    };

                }
                else {
                    var message = "You need to add your PublisherID, token, PlayerID provided by Brightcove.  Add these to the Workarea - Settings-Personalizations-YouTubeVideo widget, the variables are PlayerID, publisherID, and token";
                    $ektron(".ektronWidgetBrightcove").html("");
                    $ektron(".ektronWidgetBrightcove").append(message);
                    return;
                }
            },

            // METHODS
            AddWidget: function(id, outputId, submitButtonId) {
                // summary:
                //
                // id:
                //      
                // outputId:
                //      
                // submitButtonId:
                //      

                var widg = new Ektron.Widget.YouTubeVideo.YouTubeVideoWidget(id, outputId, submitButtonId);
                Ektron.Widget.YouTubeVideo.widgets[id] = widg;

                // Create video player list
                $ektron("#" + id + " .player-heading").hover(
                    function(evt) {
                        var playerHeading = $ektron(this).find("ul");
                        playerHeading.width($ektron(this).width());
                        playerHeading.show();
                    },
                    function() {
                        playerHeading.hide();
                    }
                );

                Ektron.Widget.YouTubeVideo.widgets[id].FindVideos();
            },

            GetWidget: function(id) {
                // summary:
                //      Return the widget 
                // id: [Interger]
                //      The index of widget

                return Ektron.Widget.YouTubeVideo.widgets[id];
            },

            Pagingbuttons: function(id, maxcount, itemIndex) {
                // summary:
                //      Create paging control
                // id: [string]
                //      The widget id
                // maxcount: [Integer]
                //      The maximum items of videos collection
                // itemIndex: [Integer]
                //      The index of item

                var numpages = 0;
                var theresults = "Results";
                var pagestart = 0;
                var pageend = parseInt(itemIndex);

                if(maxcount > 0) {
                    numpages = parseInt((maxcount - 1) / Ektron.Widget.YouTubeVideo.pageSize);
                }

                var displayClass = maxcount > Ektron.Widget.YouTubeVideo.pageSize ? '' : 'none';
                $ektron("#" + id + "First").css('display', displayClass);
                $ektron("#" + id + "Previous").css('display', displayClass);
                $ektron("#" + id + "Next").css('display', displayClass);
                $ektron("#" + id + "Last").css('display', displayClass);

                Ektron.Widget.YouTubeVideo.lastPage = numpages;
                if(Ektron.Widget.YouTubeVideo.pageNumber == 0) {
                    $ektron("#" + id + "First").attr("disabled", true).addClass("ektronWidgetYTFirstDisabled");
                    $ektron("#" + id + "Previous").attr("disabled", true).addClass("ektronWidgetYTPreviousDisabled");
                }
                else {
                    $ektron("#" + id + "First").attr("disabled", false).removeClass("ektronWidgetYTFirstDisabled");
                    $ektron("#" + id + "Previous").attr("disabled", false).removeClass("ektronWidgetYTPreviousDisabled");
                }

                if(Ektron.Widget.YouTubeVideo.pageNumber < numpages) {
                    $ektron("#" + id + "Next").attr("disabled", false).removeClass("ektronWidgetYTNextDisabled");
                    $ektron("#" + id + "Last").attr("disabled", false).removeClass("ektronWidgetYTLastDisabled");
                }
                else {
                    $ektron("#" + id + "Next").attr("disabled", true).addClass("ektronWidgetYTNextDisabled");
                    $ektron("#" + id + "Last").attr("disabled", true).addClass("ektronWidgetYTLastDisabled");;
                }

                if(maxcount > 0) {
                    pagestart = Ektron.Widget.YouTubeVideo.GetStartIndex();
                    pageend = pageend + pagestart;
                    theresults = "Results " + pagestart + " - " + pageend + " of " + maxcount;
                } else {
                    theresults = "No Results";
                }
                $ektron(".video-result").html("");
                $ektron(".video-result").append(theresults);
            },

            PagingbuttonsSearch: function(id, maxcount, itemIndex) {
                // summary:
                //      Create paging control
                // id: [string]
                //      The widget id
                // maxcount: [Integer]
                //      The maximum items of videos collection
                // itemIndex: [Integer]
                //      The index of item

                var numpages = 0;
                var theresults = "Results";
                var pagestart = 0;
                var pageend = parseInt(itemIndex);
                if(maxcount > 0) {
                    numpages = parseInt((maxcount - 1) / Ektron.Widget.YouTubeVideo.pageSize);
                }

                var displayClass = maxcount > Ektron.Widget.YouTubeVideo.pageSize ? '' : 'none';
                $ektron("#" + id + "FirstSearch").css('display', displayClass);
                $ektron("#" + id + "PreviousSearch").css('display', displayClass);
                $ektron("#" + id + "NextSearch").css('display', displayClass);
                $ektron("#" + id + "LastSearch").css('display', displayClass);

                Ektron.Widget.YouTubeVideo.lastPage = numpages;
                if(Ektron.Widget.YouTubeVideo.pageNumber == 0) {
                    $ektron("#" + id + "FirstSearch").attr("disabled", true).addClass("ektronWidgetYTFirstDisabled");
                    $ektron("#" + id + "PreviousSearch").attr("disabled", true).addClass("ektronWidgetYTPreviousDisabled");
                }
                else {
                    $ektron("#" + id + "FirstSearch").attr("disabled", false).removeClass("ektronWidgetYTFirstDisabled");
                    $ektron("#" + id + "PreviousSearch").attr("disabled", false).removeClass("ektronWidgetYTPreviousDisabled");
                }

                if(Ektron.Widget.YouTubeVideo.pageNumber < numpages) {
                    $ektron("#" + id + "NextSearch").attr("disabled", false).removeClass("ektronWidgetYTNextDisabled");
                    $ektron("#" + id + "LastSearch").attr("disabled", false).removeClass("ektronWidgetYTLastDisabled");
                }
                else {
                    $ektron("#" + id + "NextSearch").attr("disabled", true).addClass("ektronWidgetYTNextDisabled");
                    $ektron("#" + id + "LastSearch").attr("disabled", true).addClass("ektronWidgetYTLastDisabled");
                }

                if(maxcount > 0) {
                    pagestart = Ektron.Widget.YouTubeVideo.GetStartIndex();
                    pageend = pageend + pagestart;
                    theresults = "Results " + pagestart + " - " + pageend + " of " + maxcount;
                } else {
                    theresults = "No Results";
                }

                $ektron(".video-search-result").html("");
                $ektron(".video-search-result").append(theresults);
            },

            SwitchPane: function(el, panename) {
                // summary:
                //      
                // el: [string]
                //      The element id
                // panename: [string]
                //      The pane name

                var parent = $ektron(el).parents(".youtube");
                var tablist = parent.find(".ektronWidgetYTTabs li a");
                var panes = parent.children(".pane");

                for(var i = 0; i < tablist.length; i++) {
                    $ektron(tablist[i]).removeClass("selectedTab");
                    if(tablist[i].id == panename) $ektron(tablist[i]).addClass("selectedTab");
                }

                for(var i = 0; i < panes.length; i++) {
                    $ektron(panes[i]).hide();
                    if($ektron(panes[i]).hasClass(panename)) $ektron(panes[i]).show();
                }

                Ektron.Widget.YouTubeVideo.ResetPages();
            },
            ResetPages: function() {
                Ektron.Widget.YouTubeVideo.pageNumber = 0;
                Ektron.Widget.YouTubeVideo.maxCount = 0;
                Ektron.Widget.YouTubeVideo.lastPage = 0;
            }
        };
    }
});


function callBackDisplayVideos(data) {
    // summary:
    //      Display videos collection
    // data: [object]
    //      The video collection

    var objW = Ektron.Widget.YouTubeVideo.GetWidget(Ektron.Widget.YouTubeVideo.currentCallWidgetID);
    if(objW) {
        objW.DisplayVideos(data);
    }
}

function callBackDisplaySearchVideos(data) {
    // summary:
    //      Display videos collection in search area
    // data: [object]
    //      The video collection

    var objW = Ektron.Widget.YouTubeVideo.GetWidget(Ektron.Widget.YouTubeVideo.currentCallWidgetID);
    if(objW) {
        objW.DisplaySearchVideos(data);
    }
}