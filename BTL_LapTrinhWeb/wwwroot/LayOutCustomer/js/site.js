// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
 const carousel = document.querySelector('#carouselId');
    let isDragging = false;
    let startX;
    let currentIndex = 0;

    // Tự động chuyển slide
    const interval = setInterval(() => {
        currentIndex = (currentIndex + 1) % 2; // Assuming 2 slides
        bootstrap.Carousel.getInstance(carousel).next();
    }, 3000);

    // Xử lý sự kiện kéo chuột
    carousel.addEventListener('mousedown', (e) => {
        isDragging = true;
        startX = e.pageX;
    });

    carousel.addEventListener('mousemove', (e) => {
        if (!isDragging) return;
        const deltaX = e.pageX - startX;
        if (deltaX > 100) { // Kéo sang phải
            bootstrap.Carousel.getInstance(carousel).prev();
            isDragging = false; // Ngừng kéo sau khi chuyển slide
        } else if (deltaX < -100) { // Kéo sang trái
            bootstrap.Carousel.getInstance(carousel).next();
            isDragging = false; // Ngừng kéo sau khi chuyển slide
        }
    });

    carousel.addEventListener('mouseup', () => {
        isDragging = false;
    });

    carousel.addEventListener('mouseleave', () => {
        isDragging = false;
    });

//Carousel 
