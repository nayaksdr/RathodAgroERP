
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "redirectTo": "/dashboard",
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/dashboard"
  },
  {
    "renderMode": 2,
    "route": "/labors"
  },
  {
    "renderMode": 2,
    "route": "/labors/add"
  },
  {
    "renderMode": 2,
    "route": "/attendance"
  },
  {
    "renderMode": 2,
    "route": "/payments"
  },
  {
    "renderMode": 2,
    "route": "/deposits"
  },
  {
    "renderMode": 2,
    "route": "/login"
  },
  {
    "renderMode": 2,
    "route": "/register"
  },
  {
    "renderMode": 2,
    "redirectTo": "/login",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 6615, hash: '98ee92f1dfb2d70d24a5f3632aaf48227464753abfeca05abca88e80436b194e', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1013, hash: 'd803885c0a93b35b1dfb21e7253fd79901185ad388c571b1eb5c1723a8576e20', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'labors/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/labors_index_html.mjs').then(m => m.default)},
    'dashboard/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/dashboard_index_html.mjs').then(m => m.default)},
    'labors/add/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/labors_add_index_html.mjs').then(m => m.default)},
    'attendance/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/attendance_index_html.mjs').then(m => m.default)},
    'payments/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/payments_index_html.mjs').then(m => m.default)},
    'deposits/index.html': {size: 20295, hash: '84b357407ad63a814e2340c94969292ed1b783ade2e8cc65a53223dc2c0dff80', text: () => import('./assets-chunks/deposits_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 23998, hash: '7e77b03f8813ad5aef38142951aa8ccdb6f813b26db508e10618b7fb6d49a431', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'register/index.html': {size: 28578, hash: '9fd6f235699f7da1af9438a7535048be238d48fe65b29ddf4942b6889e02d90d', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'styles-DQOD7OYG.css': {size: 242208, hash: '3xo0/YdtKG0', text: () => import('./assets-chunks/styles-DQOD7OYG_css.mjs').then(m => m.default)}
  },
};
