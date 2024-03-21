





$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var query = $(this).val().trim();
        var $movieListContainer = $('#movieListContainer');
        var $movieList = $('#movieList');

        if (query.length > 0) {
            $.ajax({
                url: '/Search/SearchByTitle',
                type: 'GET',
                data: { query: query },
                success: function (data) {
                    $movieList.empty();

                    $.each(data, function (index, movie) {
                        var $div = $('<div>').addClass('search-result');
                        var $video = $('<video>').attr({ width: '100', height: '50' });
                        var $source = $('<source>').attr({ src: movie.filePath, type: 'video/mp4' });
                        var $p = $('<p>').text(movie.title);
                        var $linkDiv = $('<div>').addClass('serch-value');
                        var $a = $('<a>').attr({ href: '/Movie/DetailMovie/' + movie.id }).addClass('search-link').append('<i class="play fa-solid fa-circle-play"></i>');

                        $video.append($source);
                        $div.append($linkDiv.append($video), $linkDiv.append($p), $a);
                        $movieList.append($div);
                    });

                    $movieListContainer.show();
                }
            });
        } else {
            $movieListContainer.hide();
        }
    });
});




document.getElementById('openModalBtn').addEventListener('click', function () {
    $('#editUserModal').modal('show');
});
