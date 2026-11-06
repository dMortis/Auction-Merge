const fs = require('node:fs/promises');
const http = require('node:http');
const path = require('node:path');
const { chromium } = require('C:/Users/dimag/.cache/codex-runtimes/codex-primary-runtime/dependencies/node/node_modules/playwright');

async function main() {
  const root = 'F:/Playable Ads/Storage Auction 3/itchio/StorageAuction3-WebGL';
  const qa = 'F:/Playable Ads/Storage Auction 3/itchio/qa';
  await fs.mkdir(qa, { recursive:true });
  const server = http.createServer(async (req,res) => {
    try {
      const url = new URL(req.url, 'http://localhost');
      if (url.pathname === '/favicon.ico') { res.writeHead(204); res.end(); return; }
      const file = path.resolve(root, '.' + (url.pathname === '/' ? '/index.html' : decodeURIComponent(url.pathname)));
      if (!file.toLowerCase().startsWith(path.resolve(root).toLowerCase() + path.sep)) {
        res.writeHead(403); res.end(); return;
      }
      const body = await fs.readFile(file);
      const types = {'.html':'text/html', '.js':'text/javascript', '.wasm':'application/wasm', '.json':'application/json', '.png':'image/png'};
      res.writeHead(200, {'Content-Type':types[path.extname(file)] || 'application/octet-stream'}); res.end(body);
    } catch { res.writeHead(404); res.end(); }
  });
  await new Promise(resolve => server.listen(0, '127.0.0.1', resolve));
  let browser;
  try {
    browser = await chromium.launch({ executablePath:'C:/Program Files/Google/Chrome/Application/chrome.exe', headless:true, args:['--enable-webgl','--enable-unsafe-swiftshader'] });
    const context = await browser.newContext({viewport:{width:450,height:800},deviceScaleFactor:1});
    const page = await context.newPage();
    const errors = [], messages = [], failedRequests = [];
    page.on('pageerror', e => errors.push(e.message));
    page.on('console', m => { if(m.type()==='error') messages.push(m.text()); });
    page.on('requestfailed', r => failedRequests.push({url:r.url(),failure:r.failure()}));
    await page.goto('http://127.0.0.1:' + server.address().port, {waitUntil:'domcontentloaded'});
    await page.waitForFunction(() => window.unityInstance && document.querySelector('#loading').hidden, {timeout:120000});
    // Unity's startup splash continues briefly after createUnityInstance resolves.
    await page.waitForTimeout(6500);
    const viewports = [];
    for (const viewport of [{width:450,height:800},{width:1280,height:720},{width:375,height:812}]) {
      await page.setViewportSize(viewport);
      await page.waitForTimeout(400);
      const size = await page.locator('#unity-canvas').evaluate(el => { const r=el.getBoundingClientRect(); return {x:r.x,y:r.y,width:r.width,height:r.height,pixels:[el.width,el.height]}; });
      if (Math.abs(size.width/size.height - 9/16)>0.001) throw new Error('Wrong canvas aspect ratio: '+JSON.stringify(size));
      await page.screenshot({path:path.join(qa,`portrait-${viewport.width}x${viewport.height}.png`)});
      viewports.push({viewport,canvas:size});
    }
    const drag = process.argv.slice(2).map(Number);
    if (drag.length === 4 && drag.every(Number.isFinite)) {
      await page.setViewportSize({width:450,height:800});
      await page.waitForTimeout(400);
      await page.mouse.move(drag[0],drag[1]); await page.mouse.down();
      await page.mouse.move(drag[2],drag[3],{steps:15});
      await page.screenshot({path:path.join(qa,'drag-in-progress.png')});
      await page.mouse.up(); await page.waitForTimeout(1200);
      await page.screenshot({path:path.join(qa,'after-merge.png')});
    }
    const result = {loaded:true,viewports,errors,consoleErrors:messages,failedRequests};
    await fs.writeFile(path.join(qa,'browser-check.json'),JSON.stringify(result,null,2));
    console.log(JSON.stringify(result));
    if(errors.length || failedRequests.length) process.exitCode=1;
  } finally {
    if(browser) await browser.close();
    server.close();
  }
}
main().catch(e=>{ console.error(e); process.exitCode=1; });
