// File: wwwroot/js/receive-file-cards.js

document.addEventListener('DOMContentLoaded', function () {
    // Init tooltip nếu có jQuery + bootstrap tooltip
    if (typeof $ !== 'undefined' && typeof $.fn.tooltip === 'function') {
        $('[data-toggle="tooltip"]').tooltip();
    }

    const cards = Array.from(document.querySelectorAll('.card-file'));
    const sidebar = document.getElementById('fileDetailSidebar');

    // File icon mapping
    const fileIcons = {
        docx: "https://img.icons8.com/color/96/000000/ms-word.png",
        pptx: "https://img.icons8.com/color/96/000000/ms-powerpoint.png",
        xlsx: "https://img.icons8.com/color/96/000000/ms-excel.png",
        pdf: "https://img.icons8.com/color/96/000000/pdf.png",
        jpg: "https://img.icons8.com/color/96/000000/image.png",
        png: "https://img.icons8.com/color/96/000000/image.png",
        default: "https://img.icons8.com/fluency/96/000000/file.png"
    };

    // Set thumbnails
    cards.forEach(card => {
        const type = (card.dataset.type || '').toLowerCase();
        const img = card.querySelector('.cover');
        if (img) img.src = fileIcons[type] || fileIcons.default;

        // Hide other-actions initially
        const other = card.querySelector('.other-actions');
        if (other) other.style.display = 'none';
    });

    // Helper: reset selection
    function resetSelection() {
        document.querySelectorAll('.card-file.active').forEach(c => {
            c.classList.remove('active');
            const o = c.querySelector('.other-actions');
            if (o) o.style.display = 'none';
        });
    }

    // Show sidebar with data
    function showSidebar(data) {
        if (!sidebar) return;

        const fileNameEl = document.getElementById('fileName');
        const fileOwnerEl = document.getElementById('fileOwner');
        const fileEmailEl = document.getElementById('fileEmail');
        const fileModifiedEl = document.getElementById('fileModified');

        if (fileNameEl) fileNameEl.textContent = data.name || '';
        if (fileOwnerEl) fileOwnerEl.textContent = data.owner || '';
        if (fileEmailEl) fileEmailEl.textContent = data.email || '';
        if (fileModifiedEl) fileModifiedEl.textContent = data.date || '';

        sidebar.classList.add('show');
    }

    // Click vào nút 3 chấm để hiện sidebar
    document.addEventListener('click', function (evt) {
        const detailsBtn = evt.target.closest('.btn-details');
        if (!detailsBtn) return;

        evt.stopPropagation();
        resetSelection();

        const card = detailsBtn.closest('.card-file');
        if (card) {
            card.classList.add('active');
            const other = card.querySelector('.other-actions');
            if (other) other.style.display = 'flex';

            showSidebar({
                name: card.dataset.filename || '',
                owner: card.dataset.sharedby || '',
                email: card.dataset.email || '',
                date: card.dataset.dateshared || ''
            });
        }
    });

    // Click trên card để chọn (không hiện sidebar)
    cards.forEach(card => {
        card.addEventListener('click', function (e) {
            // Bỏ qua nếu click vào button/a/dropdown
            if (e.target.closest('button') || e.target.closest('a') ||
                e.target.closest('[data-toggle="dropdown"], .dropdown-toggle')) return;

            resetSelection();
            this.classList.add('active');
            const other = this.querySelector('.other-actions');
            if (other) other.style.display = 'flex';
        });
    });

    // Double click để mở file
    cards.forEach(card => {
        card.addEventListener('dblclick', function (e) {
            // Bỏ qua nếu double click vào button/dropdown
            if (e.target.closest('button') || e.target.closest('.dropdown-menu')) {
                return;
            }

            const fileUrl = card.dataset.url;
            if (fileUrl) {
                window.open(fileUrl, '_blank');
            }
        });
    });

    // Close sidebar button
    if (sidebar) {
        const closeBtn = sidebar.querySelector('.close');
        if (closeBtn) {
            closeBtn.addEventListener('click', function (e) {
                e.stopPropagation();
                sidebar.classList.remove('show');
            });
        }
    }

    // Click ra ngoài để reset selection (không đóng sidebar)
    document.addEventListener('click', function (evt) {
        const clickedInCard = !!evt.target.closest('.card-file');
        const clickedInSidebar = !!evt.target.closest('#fileDetailSidebar');
        const clickedInDropdown = !!evt.target.closest('.dropdown-menu') ||
            !!evt.target.closest('[data-toggle="dropdown"], .dropdown-toggle');

        if (!clickedInCard && !clickedInSidebar && !clickedInDropdown) {
            resetSelection();
        }
    });
});