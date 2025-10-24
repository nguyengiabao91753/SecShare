window.receiveSidebar = (function () {
    function ensureAttached() {
        const sidebar = document.getElementById('fileDetailSidebar');
        if (!sidebar) return;

        // nếu đã attach dưới body thì thôi
        if (sidebar.parentElement === document.body) return;

        // lưu thông tin vị trí cũ để có thể restore nếu muốn
        sidebar.__origParent = sidebar.parentElement;
        sidebar.__origNext = sidebar.nextSibling;

        document.body.appendChild(sidebar);
    }

    function open(width) {
        ensureAttached();
        if (width) {
            // tùy chọn: set CSS variable (nếu bạn muốn dùng var(--sidebar-width))
            document.documentElement.style.setProperty('--sidebar-width', width);
        }
        document.body.classList.add('with-sidebar');
        const sidebar = document.getElementById('fileDetailSidebar');
        if (sidebar) {
            sidebar.classList.add('show');
            // chắc chắn fixed, override nếu bị CSS khác
            sidebar.style.position = 'fixed';
            sidebar.style.right = '0';
            sidebar.style.top = '0';
        }
    }

    function close() {
        const sidebar = document.getElementById('fileDetailSidebar');
        if (sidebar) {
            sidebar.classList.remove('show');
            // không restore position here; keep it detached (safe). If restore needed, call restore().
        }
        document.body.classList.remove('with-sidebar');
    }

    function restore() {
        const sidebar = document.getElementById('fileDetailSidebar');
        if (!sidebar) return;
        if (sidebar.__origParent) {
            if (sidebar.__origNext) sidebar.__origParent.insertBefore(sidebar, sidebar.__origNext);
            else sidebar.__origParent.appendChild(sidebar);
            delete sidebar.__origParent;
            delete sidebar.__origNext;
        }
        // remove inline styles we set
        sidebar.style.position = '';
        sidebar.style.right = '';
        sidebar.style.top = '';
        sidebar.classList.remove('show');
        document.body.classList.remove('with-sidebar');
    }

    // Observer fallback: keep body class sync với sidebar.show
    (function observe() {
        const sidebar = document.getElementById('fileDetailSidebar');
        if (!sidebar) return;
        const mo = new MutationObserver(function (muts) {
            muts.forEach(m => {
                if (m.attributeName === 'class') {
                    if (sidebar.classList.contains('show')) document.body.classList.add('with-sidebar');
                    else document.body.classList.remove('with-sidebar');
                }
            });
        });
        mo.observe(sidebar, { attributes: true, attributeFilter: ['class'] });
    })();

    return {
        ensureAttached,
        open,
        close,
        restore
    };
})();
