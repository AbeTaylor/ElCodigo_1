define(["sitecore"], function (Sitecore) {
    var FavoritesItemTreeViewPageCode = Sitecore.Definitions.App.extend({
        FavStructure: [],

        FavItemId: '',

        FavRootId: "{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}",

        initialized: function () {
            var that = this;

            this.runFavoritesAction("getRoot").visit(function (node) {
                window.setInterval(function () { node.expand(true); }, 50);
            });

            this.FavItemId = window.top.ExperienceEditor.instance.currentContext.itemId;


            this.initializeFavStructure();
            this.initializeFavTree(that);
        },

        runFavoritesAction: function (action) {
            return $("div[data-sc-id='FavoritesNav']").dynatree(action);
        },

        initializeFavStructure: function () {
            var context = window.top.ExperienceEditor.generateDefaultContext();
            context.currentContext.itemId = this.FavItemId;

            var that = this;

            window.top.ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.Breadcrumb.GetStructure", function (response) {
                if (!response.responseValue.value) {
                    return;
                }

                that.FavStructure = response.responseValue.value;
            }).execute(context);
        },

        initializeFavTree: function (context) {
            this.runFavoritesAction({
                onActivate: function (node) {
                    if (node.data.isDisabledState) {
                        return;
                    }

                    context.navigateToItem(node.data.key);
                },

                onCreate: function (node) {
                    context.setNodeActiveStatus(node);
                    context.checkExpandingStatus(node, context.FavStructure, decodeURIComponent(context.FavItemId));
                },

                onRender: function (node) {
                    if (node.data.key == context.FavRootId) {
                        var rootElement = node.span;
                        if (!rootElement) {
                            return;
                        }

                        var $root = $(rootElement);
                        $root.addClass("disabledItem");

                        node.data.isDisabledState = true;
                    }
                }
            });
        },

        setNodeActiveStatus: function (node) {
            node.data.isDisabledState = false;
            var context = window.top.ExperienceEditor.generateDefaultContext();
            context.currentContext.value = node.data.key;

            window.top.ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.Favorites.GetFavoriteItem", function (response) {
                if (!response.responseValue.value) {
                    node.data.addClass = "disabledItem";

                    node.data.isDisabledState = true;
                    return;
                }

                context.currentContext.value = response.responseValue.value;
            }).execute(context);

            window.top.ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.Item.HasPresentation", function (response) {
                if (!response.responseValue.value) {
                    node.data.addClass = "disabledItem";

                    node.data.isDisabledState = true;
                }
            }).execute(context);
        },

        checkExpandingStatus: function (node, structure, currentItemId) {
            var itemExistsInStructure = this.isItemExistsInStructure(node.data.key, structure);
            if (!itemExistsInStructure) {
                return;
            }

            var nodeIsCurrentItem = node.data.key == currentItemId;

            if (nodeIsCurrentItem) {
                node.data.addClass = "dynatree-active";

                return;
            }
            node.expand(true);
        },

        isItemExistsInStructure: function (itemId, structure) {
            for (var i = 0; i < structure.length; i++) {
                if (structure[i].ItemId == itemId) {
                    return true;
                }
            }

            return false;
        },

        navigateToItem: function (itemId) {
            var context = window.top.ExperienceEditor.generateDefaultContext();
            context.currentContext.value = itemId;
            var editorContext = window.top.ExperienceEditor;
            var favItemId;

            window.top.ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.Favorites.GetFavoriteItem", function (response) {
                if (!response.responseValue.value) {
                    return;
                }

                favItemId = response.responseValue.value;
            }).execute(context);

            editorContext.handleIsModified();
            editorContext.navigateToItem(favItemId);
        }
    });

    return FavoritesItemTreeViewPageCode;
});