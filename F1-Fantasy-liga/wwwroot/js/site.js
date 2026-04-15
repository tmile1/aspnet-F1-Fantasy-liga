// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(() => {
  const isInteractiveTarget = (target) => {
    return !!target.closest("a, button, input, select, textarea, label");
  };

  document.addEventListener("click", (event) => {
    const row = event.target.closest(".row-link[data-href]");
    if (!row || isInteractiveTarget(event.target)) {
      return;
    }

    window.location.href = row.dataset.href;
  });

  document.addEventListener("keydown", (event) => {
    const row = event.target.closest(".row-link[data-href]");
    if (!row || (event.key !== "Enter" && event.key !== " ")) {
      return;
    }

    event.preventDefault();
    window.location.href = row.dataset.href;
  });
})();
