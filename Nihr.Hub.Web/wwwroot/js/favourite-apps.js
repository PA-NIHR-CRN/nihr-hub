document.addEventListener("DOMContentLoaded", function () {

    const listOne = document.getElementById('favourites-list');
    const listTwo = document.getElementById('all-applications');
    const antiforgeryToken = document.querySelector('meta[name="request-verification-token"]')?.content;

    new Sortable(listOne, {
        group: 'favourites', // set both lists to same group
        animation: 150, onSort: function (event) {
            let items = Array.from(event.target.children).map(item => item.dataset.id);
            const headers = {
                'Content-Type': 'application/json'
            };

            if (antiforgeryToken) {
                headers.RequestVerificationToken = antiforgeryToken;
            }

            fetch('/save-favourites', {
                method: 'POST',
                headers,
                body: JSON.stringify({favouriteIds: items}) // Send as JSON
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
