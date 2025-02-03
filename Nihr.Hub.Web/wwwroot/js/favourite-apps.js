document.addEventListener("DOMContentLoaded", function () {

    const listOne = document.getElementById('favourites-list');
    const listTwo = document.getElementById('all-applications');

    new Sortable(listOne, {
        group: 'favourites', // set both lists to same group
        animation: 150, onSort: function (event) {
            let items = Array.from(event.target.children).map(item => item.dataset.id);
            fetch('/save-favourites', {
                method: 'POST', headers: {
                    'Content-Type': 'application/json'
                }, body: JSON.stringify({favouriteIds: items}) // Send as JSON
            }).then(response => {
                if (!response.ok) {
                    throw new Error('Failed to save favourites');
                }
            });
        }
    });

    new Sortable(listTwo, {
        group: 'favourites', animation: 150, sort: false
    });
})
;
