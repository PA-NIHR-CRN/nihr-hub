document.addEventListener("DOMContentLoaded", function () {

    const listOne = document.getElementById('list-one');
    const listTwo = document.getElementById('list-two');

    new Sortable(listOne, {
        group: 'favourites', // set both lists to same group
        animation: 150
    });

    new Sortable(listTwo, {
        group: 'favourites',
        animation: 150,
        sort: false
    });
});
